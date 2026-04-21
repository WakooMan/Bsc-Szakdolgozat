import gymnasium as gym
from sb3_contrib import MaskablePPO
from sb3_contrib.common.maskable.callbacks import MaskableEvalCallback
from sb3_contrib.common.wrappers import ActionMasker
from stable_baselines3.common.callbacks import CallbackList
from stable_baselines3.common.monitor import Monitor
import numpy as np
from SevenWondersEnv import SevenWondersEnv


def mask_fn(env) -> np.ndarray:
    return np.array(env.get_wrapper_attr("last_mask"), dtype=np.float32)


class MaskableSevenWondersEnv(SevenWondersEnv):
    """Wrapper that stores the action mask for sb3-contrib's ActionMasker."""

    def __init__(self):
        super().__init__()
        self.last_mask = np.ones(self.action_space.n, dtype=np.float32)

    def reset(self, seed=None, options=None):
        obs, info = super().reset(seed=seed, options=options)
        self.last_mask = np.array(info.get("mask", np.ones(self.action_space.n)), dtype=np.float32)
        return obs, info

    def step(self, action):
        obs, reward, terminated, truncated, info = super().step(action)
        self.last_mask = np.array(info.get("mask", np.ones(self.action_space.n)), dtype=np.float32)
        return obs, reward, terminated, truncated, info


def main():
    env = MaskableSevenWondersEnv()
    env = Monitor(env)
    env = ActionMasker(env, mask_fn)

    model = MaskablePPO(
        "MlpPolicy",
        env,
        verbose=1,
        tensorboard_log="./logs/seven_wonders_ppo",
        learning_rate=3e-4,
        n_steps=2048,
        batch_size=64,
        n_epochs=10,
        gamma=0.99,
    )

    print("Starting training...")
    model.learn(total_timesteps=500_000)

    model.save("seven_wonders_agent")
    print("Model saved.")

    env.close()


if __name__ == "__main__":
    main()