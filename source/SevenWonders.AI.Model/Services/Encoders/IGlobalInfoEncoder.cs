namespace SevenWonders.AI.Model.Services.Encoders
{
    public interface IGlobalInfoEncoder
    {
        void EncodeGlobalInfo(List<float> vector, PhaseIndicator phaseIndicator);
    }
}
