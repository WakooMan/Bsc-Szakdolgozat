namespace SevenWondersUI.Services
{
    public interface IAIModelHandler
    {
        Task Initialize();
        void LoadModel(AIModelType aIModel);
    }
}
