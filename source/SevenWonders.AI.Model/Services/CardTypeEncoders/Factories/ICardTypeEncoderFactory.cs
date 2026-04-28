namespace SevenWonders.AI.Model.Services.CardTypeEncoders.Factories
{
    public interface ICardTypeEncoderFactory
    {
        ICardTypeEncoder? Create(Type cardType);
    }
}
