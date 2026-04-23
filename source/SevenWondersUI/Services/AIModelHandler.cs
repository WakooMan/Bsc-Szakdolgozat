using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SevenWonders.AI.Model.DecisionRouter.DecisionHandlers;
using SevenWonders.AI.Model.Messages;

namespace SevenWondersUI.Services
{
    public class AIModelHandler : IAIModelHandler
    {
        public AIModelHandler(IAIDecisionHandler aIDecisionHandler)
        {
            m_aIDecisionHandler = aIDecisionHandler;
            m_session = null;
            m_aiModels = new Dictionary<AIModelType, string>
            {
                { AIModelType.Easy, "seven_wonders_easy_agent.onnx" }
            };
        }

        public async Task Initialize()
        {
            foreach (var model in m_aiModels)
            {
                await CopyModelAsync($@"{model.Value}");
                await CopyModelAsync($@"{model.Value}.data");
            }
        }

        public void LoadModel(AIModelType aIModel)
        {
            m_session?.Dispose();
            string modelPath = Path.Combine(m_appDataDirectory, m_aiModels[aIModel]);
            m_session = new InferenceSession(modelPath);

            m_aIDecisionHandler.OnGameStateReceived += GameStateReceived;
        }

        private ActionRequest GameStateReceived(GameStateResponse response)
        {
            if (m_session is null)
            {
                return new ActionRequest() { Action = -1 };
            }

            var obsTensor = new DenseTensor<float>(
                response.State.ToArray(),
                new[] { 1, response.State.Count });

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("observation", obsTensor)
            };

            using var results = m_session.Run(inputs);
            var logits = results[0]
                .AsTensor<float>()
                .ToArray();

            var mask = response.Mask.ToArray();

            for (int i = 0; i < logits.Length; i++)
            {
                if (mask[i] == 0)
                {
                    logits[i] = float.MinValue;
                }
            }

            int action = Array.IndexOf(logits, logits.Max());

            if (logits[action] == float.MinValue)
            {
                action = Array.IndexOf(mask, 1);
            }

            return new ActionRequest() { Action = action };
        }

        private async Task CopyModelAsync(string modelName)
        {
            var targetPath = Path.Combine(m_appDataDirectory, modelName);

            if (!File.Exists(targetPath))
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(modelName);
                using var fileStream = File.Create(targetPath);
                await stream.CopyToAsync(fileStream);
            }

        }

        private InferenceSession? m_session;
        private readonly IDictionary<AIModelType, string> m_aiModels;
        private readonly IAIDecisionHandler m_aIDecisionHandler;
        private readonly string m_appDataDirectory = FileSystem.AppDataDirectory;
    }
}
