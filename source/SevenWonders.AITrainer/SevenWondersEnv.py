import gymnasium as gym
from gymnasium import spaces
import numpy as np
import socket
import json


class SevenWondersEnv(gym.Env):
    def __init__(self):
        super(SevenWondersEnv, self).__init__()

        self.action_space = spaces.Discrete(23)
        self.observation_space = spaces.Box(low=-np.inf, high=np.inf, shape=(619,), dtype=np.float32)

        self.client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.client_socket.connect(('127.0.0.1', 5000))
        self._buffer = ""
        self._opponent_type = -1

    def _send_message(self, message_type: str, payload: object = None):
        """Send a BaseMessage to the C# server."""
        payload_json = json.dumps(payload) if payload is not None else ""
        base_message = {
            "MessageType": _MESSAGE_TYPE_MAP[message_type],
            "Payload": payload_json
        }
        self.client_socket.sendall(json.dumps(base_message).encode() + b"\n")

    def _receive_message(self) -> dict:
        """Receive a complete line-delimited BaseMessage from the C# server."""
        while "\n" not in self._buffer:
            chunk = self.client_socket.recv(8192).decode()
            if not chunk:
                raise ConnectionError("Server closed connection")
            self._buffer += chunk

        line, self._buffer = self._buffer.split("\n", 1)
        base_message = json.loads(line.strip())
        payload = json.loads(base_message["Payload"]) if base_message.get("Payload") else {}
        return {
            "MessageType": base_message["MessageType"],
            "Payload": payload
        }

    def reset(self, seed=None, options=None):
        super().reset(seed=seed)

        self._send_message("ResetRequest")

        msg = self._receive_message()
        payload = msg["Payload"]
        observation = np.array(payload["state"], dtype=np.float32)
        mask = payload.get("mask", [])
        self._opponent_type = payload.get("opponent_type", -1)

        return observation, {"mask": mask, "opponent_type": self._opponent_type}

    def step(self, action):
        self._send_message("ActionRequest", {"action": int(action)})

        msg = self._receive_message()
        payload = msg["Payload"]

        terminated = payload.get("terminated", False)
        reward = payload.get("reward", 0.0)
        mask = payload.get("mask", [])

        if terminated:
            obs = np.zeros(self.observation_space.shape, dtype=np.float32)
            return obs, reward, True, False, {"won": reward > 0, "opponent_type": self._opponent_type}

        obs = np.array(payload["state"], dtype=np.float32)
        return obs, reward, False, False, {"mask": mask}

    def close(self):
        try:
            self._send_message("ExitRequest")
        except OSError:
            pass
        self.client_socket.close()

_MESSAGE_TYPE_MAP = {
    "ActionRequest": 0,
    "GameStateResponse": 1,
    "ResetRequest": 2,
    "GameResetResponse": 3,
    "ExitRequest": 4
}