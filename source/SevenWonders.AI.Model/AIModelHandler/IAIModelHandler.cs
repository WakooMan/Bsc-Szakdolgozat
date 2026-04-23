namespace SevenWonders.AI.Model.AIModelHandler
{
    public interface IAIModelHandler
    {
        Task Initialize();
        void LoadModel(AIModelType aIModel);
    }
}
