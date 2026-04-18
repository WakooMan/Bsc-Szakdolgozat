import gymnasium as gym
from gymnasium import spaces
import numpy as np
import socket

class SevenWondersEnv(gym.Env):
    def __init__(self):
        super(SevenWondersEnv, self).__init__()

        self.action_space = spaces.Discrete(23)

        self.observation_space = spaces.Box(low=0, high=1, shape=(50,), dtype=np.float32)

        self.client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.client_socket.connect(('127.0.0.1', 5000))

    def reset(self, seed=None, options=None):
        super().reset(seed=seed)
        
        self.client_socket.sendall(b"RESET")
        
        data = self.client_socket.recv(4096)
        observation = self._parse_observation(data)
        
        return observation, {}

    def step(self, action):
        self.client_socket.sendall(str(action).encode())

        data = self.client_socket.recv(4096).decode().split('|')
        
        obs = self._parse_observation(data[0])
        reward = float(data[1])
        terminated = data[2].lower() == 'true'
        truncated = False 

        return obs, reward, terminated, truncated, {}

    def _parse_observation(self, data):
        return np.array(eval(data), dtype=np.float32)

    def close(self):
        self.client_socket.close()