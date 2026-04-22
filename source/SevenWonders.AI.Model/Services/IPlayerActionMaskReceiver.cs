using GameLogic.GameStructures;
using GameLogic.Interfaces;

namespace SevenWonders.AI.Model.Services
{
    public interface IPlayerActionMaskReceiver
    {
        void Initialize();
        List<int> ReceivePlayerActionMask(PhaseIndicator phaseIndicator, PlayerActionWrapper[] playerActions);
        List<int> ReceiveEmptyPlayerActionMask();

        ICardNode? GetNode(int index);
    }
}
