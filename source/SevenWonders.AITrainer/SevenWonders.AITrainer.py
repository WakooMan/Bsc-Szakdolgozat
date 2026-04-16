import gymnasium as gym
from stable_baselines3 import PPO
from SevenWondersEnv import SevenWondersEnv

def main():
    env = SevenWondersEnv()

    print("Connected to server. Choosable mode: 1 - Learning, 2 - Game")
    mode = input("Choose mode: ")

    if mode == "1":
        model = PPO("MlpPolicy", env, verbose=1, tensorboard_log="./ppo_duel_logs/")
        
        print("Learning starts...")
        model.learn(total_timesteps=100000, progress_bar=True)
        
        model.save("7w_duel_model_v1")
        print("Modell elmentve.")

    else:
        model = PPO.load("7w_duel_model_v1")
        
        obs, _ = env.reset()
        print("Starting the match against the AI")
        
        while True:
            action, _states = model.predict(obs, deterministic=True)
            
            obs, reward, terminated, truncated, info = env.step(action)
            
            if terminated or truncated:
                print("Match end.")
                break

    env.close()

if __name__ == "__main__":
    main()