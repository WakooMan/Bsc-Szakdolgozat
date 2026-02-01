using SevenWonders.GameEngine;
using System.Numerics;

namespace SevenWonders.Presenter
{
    public interface IWonderView
    {
        void MoveGameObjectTo(GameObject gameObject, Vector2 target);

        void HighlightGameObject(GameObject gameObject);

        void LiftGameObject(GameObject gameObject);

    }
}
