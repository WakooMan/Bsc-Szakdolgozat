import gymnasium as gym
import json
import signal
import sys
import torch
from sb3_contrib import MaskablePPO
from sb3_contrib.common.maskable.callbacks import MaskableEvalCallback
from sb3_contrib.common.wrappers import ActionMasker
from stable_baselines3.common.callbacks import BaseCallback, CallbackList
from stable_baselines3.common.monitor import Monitor
import numpy as np
from SevenWondersEnv import SevenWondersEnv


def load_config(path="trainer_config.json") -> dict:
    with open(path, "r") as f:
        return json.load(f)


def mask_fn(env) -> np.ndarray:
    return np.array(env.get_wrapper_attr("last_mask"), dtype=bool)


class MaskableSevenWondersEnv(SevenWondersEnv):
    """Wrapper that stores the action mask for sb3-contrib's ActionMasker."""

    def __init__(self):
        super().__init__()
        self.last_mask = np.ones(self.action_space.n, dtype=bool)

    def reset(self, seed=None, options=None):
        obs, info = super().reset(seed=seed, options=options)
        self.last_mask = np.array(info.get("mask", np.ones(self.action_space.n)), dtype=bool)
        return obs, info

    def step(self, action):
        obs, reward, terminated, truncated, info = super().step(action)
        self.last_mask = np.array(info.get("mask", np.ones(self.action_space.n)), dtype=bool)
        return obs, reward, terminated, truncated, info


class GracefulExitCallback(BaseCallback):
    """Stops training on SIGINT/Ctrl+C and saves the model."""

    def __init__(self, save_path: str, verbose=0):
        super().__init__(verbose)
        self.save_path = save_path
        self._exit_requested = False
        signal.signal(signal.SIGINT, self._signal_handler)

    def _signal_handler(self, signum, frame):
        print("\nCancellation requested. Finishing current step and saving model...")
        self._exit_requested = True

    def _on_step(self) -> bool:
        if self._exit_requested:
            self.model.save(self.save_path)
            print(f"Model saved to '{self.save_path}' after cancellation.")
            return False
        return True


OPPONENT_TYPE_NAMES = {
    0: "RandomBot",
    1: "MilitaryHeuristicBot",
    2: "ScientificHeuristicBot",
    3: "CitizenHeuristicBot",
    4: "EasyAIModel",
    5: "MediumAIModel"
}


class WinRateCallback(BaseCallback):
    """Tracks total and per-opponent-type win rates and logs them to TensorBoard."""

    def __init__(self, verbose=0):
        super().__init__(verbose)
        self.wins = 0
        self.episodes = 0
        self.wins_by_type = {}
        self.episodes_by_type = {}

    def _on_step(self) -> bool:
        infos = self.locals.get("infos", [])
        for info in infos:
            if "episode" in info:
                self.episodes += 1
                won = info.get("won", False)
                if won:
                    self.wins += 1

                win_rate = self.wins / self.episodes
                self.logger.record("game/win_rate", win_rate)
                self.logger.record("game/total_wins", self.wins)
                self.logger.record("game/total_episodes", self.episodes)

                opponent_type = info.get("opponent_type", -1)
                if opponent_type >= 0:
                    self.episodes_by_type[opponent_type] = self.episodes_by_type.get(opponent_type, 0) + 1
                    if won:
                        self.wins_by_type[opponent_type] = self.wins_by_type.get(opponent_type, 0) + 1
                    type_name = OPPONENT_TYPE_NAMES.get(opponent_type, f"type_{opponent_type}")
                    type_wins = self.wins_by_type.get(opponent_type, 0)
                    type_episodes = self.episodes_by_type[opponent_type]
                    self.logger.record(f"game/win_rate_vs_{type_name}", type_wins / type_episodes)
                    self.logger.record(f"game/episodes_vs_{type_name}", type_episodes)
        return True


def main():
    config = load_config()
    total_timesteps = config.get("total_timesteps", 500_000)
    starter_model = config.get("starter_model", None)
    result_model = config.get("result_model", "seven_wonders_agent")
    reset_num_timesteps = config.get("reset_num_timesteps", False)
    tb_log_name = config.get("tb_log_name", "MaskablePPO")
    ent_coef = config.get("ent_coef", 0.0005)
    learning_rate = config.get("learning_rate", 0.00001)
    device = "cuda" if torch.cuda.is_available() else "cpu"
    print(f"Learning with: {device}")

    env = MaskableSevenWondersEnv()
    env = Monitor(env)
    env = ActionMasker(env, mask_fn)

    if starter_model:
        custom_objects = {
            "learning_rate": learning_rate,
            "ent_coef": ent_coef
        }
        print(f"Loading starter model from '{starter_model}'...")
        model = MaskablePPO.load(starter_model, env=env, custom_objects=custom_objects, device=device, tensorboard_log="./logs/seven_wonders_ppo")
    else:
        policy_kwargs = dict(
            net_arch=[512, 512, 512]
        )
        model = MaskablePPO(
            "MlpPolicy",
            env,
            verbose=1,
            tensorboard_log="./logs/seven_wonders_ppo",
            device=device,
            learning_rate=learning_rate,
            policy_kwargs=policy_kwargs, 
            ent_coef=ent_coef,
            n_steps=4096,
            batch_size=512,
            n_epochs=10,
            gamma=0.99,
        )

    callbacks = CallbackList([
        GracefulExitCallback(save_path=result_model),
        WinRateCallback(),
    ])

    print(f"Starting training for {total_timesteps} timesteps...")
    model.learn(total_timesteps=total_timesteps, 
                reset_num_timesteps=reset_num_timesteps, 
                tb_log_name=tb_log_name, 
                callback=callbacks)

    model.save(result_model)
    print(f"Model saved to '{result_model}'.")

    env.close()


if __name__ == "__main__":
    main()