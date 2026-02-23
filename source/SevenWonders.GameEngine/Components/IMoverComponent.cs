namespace SevenWonders.GameEngine.Components
{
    public interface IMoverComponent: IComponent
    {
        void MoveTo(GameObject gameObject, GameObject target, float movementSpeed, float rotationSpeed);
    }
}
