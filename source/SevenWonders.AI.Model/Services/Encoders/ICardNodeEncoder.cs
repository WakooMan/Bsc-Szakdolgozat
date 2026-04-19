using GameLogic.GameStructures;

using GameLogic.Elements;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public interface ICardNodeEncoder
    {
        /// <summary>
        /// Encodes a card node into a vector representation.
        /// </summary>
        /// <param name="vector">The list to which the encoded card node will be added.</param>
        /// <param name="cardNode">The card node to encode.</param>
        /// <param name="isAvailable">Indicates whether the card is available for play.</param>
        /// <param name="ownerProperties">The properties of the player from whose perspective the cost is calculated.</param>
        void EncodeCardNode(List<float> vector, ICardNode cardNode, bool isAvailable, PlayerProperties ownerProperties, PlayerProperties opponentProperties);
        void EncodeEmptyCardNode(List<float> vector);
    }
}
