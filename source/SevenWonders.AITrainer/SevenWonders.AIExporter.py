import torch
import numpy as np
from sb3_contrib import MaskablePPO


def main():
    model_path = "seven_wonders_agent"
    onnx_path = "seven_wonders_agent.onnx"
    obs_size = 1432

    print(f"Loading model from '{model_path}'...")
    model = MaskablePPO.load(model_path)

    policy = model.policy
    policy.set_training_mode(False)

    class PolicyWrapper(torch.nn.Module):
        """Wraps the SB3 policy into a simple obs -> action_logits module."""
        def __init__(self, policy):
            super().__init__()
            self.features_extractor = policy.features_extractor
            self.mlp_extractor = policy.mlp_extractor
            self.action_net = policy.action_net

        def forward(self, obs):
            features = self.features_extractor(obs)
            latent_pi, _ = self.mlp_extractor(features)
            action_logits = self.action_net(latent_pi)
            return action_logits

    wrapper = PolicyWrapper(policy)
    wrapper.eval()

    dummy_input = torch.zeros(1, obs_size, dtype=torch.float32)

    torch.onnx.export(
        wrapper,
        dummy_input,
        onnx_path,
        input_names=["observation"],
        output_names=["action_logits"],
        dynamic_axes={
            "observation": {0: "batch_size"},
            "action_logits": {0: "batch_size"},
        },
        opset_version=17,
    )


if __name__ == "__main__":
    main()
