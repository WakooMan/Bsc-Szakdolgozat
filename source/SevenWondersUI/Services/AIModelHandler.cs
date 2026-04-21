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
            m_aiModels = new Dictionary<AIModelType, string>
            {
                { AIModelType.Easy, "seven_wonders_easy_agent.onnx" }
            };
        }

        public async Task Initialize()
        {
            foreach (var model in m_aiModels)
            {
                await CopyModelAsync($@"Resources\Models\{model.Value}");
                await CopyModelAsync($@"Resources\Models\{model.Value}.data");
            }
        }

        public void LoadModel(AIModelType aIModel)
        {
            string modelPath = Path.Combine(m_appDataDirectory, m_aiModels[aIModel]);
            var m_session = new InferenceSession(modelPath);

            m_aIDecisionHandler.OnGameStateReceived += GameStateReceived;
        }

        private void GameStateReceived(GameStateResponse response)
        {
            if(m_session is null)
            {
                 return;
            }

            var obsTensor = new DenseTensor<float>(
                response.State.ToArray(),
                new[] { 1, response.State.Count });


            var maskTensor = new DenseTensor<float>(
                response.Mask.Select(num => (float)num).ToArray(),
                new[] { 1, response.Mask.Count });



            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("observation", obsTensor),
                NamedOnnxValue.CreateFromTensor("mask", maskTensor),
            };

            using var results = m_session.Run(inputs);

            var logits = results[0]
                .AsTensor<float>()
                .ToArray();

            int action = Array.IndexOf(logits, logits.Max());
            m_aIDecisionHandler.Decide(new ActionRequest() { Action = action });
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
