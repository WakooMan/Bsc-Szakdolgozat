namespace SevenWonders.GameEngine
{
    public struct Movement
    {
        public Movement(GameObject gameObject, GameObject target, float movementSpeed, float rotationSpeed)
        {
            GameObject = gameObject;
            Target = target;
            MovementSpeed = movementSpeed;
            RotationSpeed = rotationSpeed;
        }

        public GameObject GameObject { get; set; }
        public GameObject Target { get; set; }
        public float MovementSpeed { get; set; }
        public float RotationSpeed { get; set; }
    }
}