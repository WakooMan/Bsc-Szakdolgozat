using System.Numerics;

namespace SevenWonders.GameEngine.Components
{
    public class MoverComponent : IMoverComponent
    {
        public MoverComponent()
        {
            Id = 100;
            Name = nameof(MoverComponent);
            m_movements = new List<Movement>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public void Shutdown()
        {
            m_movements.Clear();
        }

        public void Startup()
        {
            m_movements.Clear();
        }

        public void Update(float deltaTime)
        {
            lock (m_movements)
            {
                List<Movement> movementsToRemove = new List<Movement>();
                foreach (var movement in m_movements)
                {
                    if (movement.GameObject.Position != movement.Target.Position)
                    {
                        movement.GameObject.Position = GetNewPositionForGameObject(movement.GameObject.Position, movement.Target.Position, movement.MovementSpeed, deltaTime);
                    }

                    if (MathF.Abs(movement.GameObject.Rotation - movement.Target.Rotation) > 1e-6f)
                    {
                        movement.GameObject.Rotation = GetNewRotation(movement.GameObject.Rotation, movement.Target.Rotation, movement.RotationSpeed, deltaTime, 0.01f);
                    }

                    if (movement.GameObject.Position == movement.Target.Position && MathF.Abs(movement.GameObject.Rotation - movement.Target.Rotation) < 1e-6f)
                    {
                        movementsToRemove.Add(movement);
                    }
                }

                foreach (var movementToRemove in movementsToRemove)
                {
                    m_movements.Remove(movementToRemove);
                }
            }
        }

        public void MoveTo(GameObject gameObject, GameObject target, float movementSpeed, float rotationSpeed)
        {
            lock (m_movements)
            {
                m_movements.Add(new Movement(gameObject, target, movementSpeed, rotationSpeed));
            }
        }

        private float GetNewRotation(float currentRotation, float targetRotation, float rotationSpeed, float deltaTime, float threshold)
        {
            float diff = targetRotation - currentRotation;
            diff = (diff + 180) % 360;
            if (diff < 0) diff += 360;
            diff -= 180;

            if (Math.Abs(diff) < threshold)
            {
                float normalizedTarget = targetRotation % 360;
                if (normalizedTarget < 0) normalizedTarget += 360;
                return normalizedTarget;
            }

            float maxStep = rotationSpeed * deltaTime;
            float actualStep;

            if (Math.Abs(diff) < maxStep)
            {
                actualStep = diff;
            }
            else
            {
                actualStep = Math.Sign(diff) * maxStep;
            }

            float newRotation = (currentRotation + actualStep) % 360;
            if (newRotation < 0) newRotation += 360;

            return newRotation;
        }

        private Vector2 GetNewPositionForGameObject(Vector2 currentPosition, Vector2 targetPosition, float movementSpeed, float deltaTime)
        {
            Vector2 direction = targetPosition - currentPosition;

            float distance = direction.Length();

            if (distance <= 0.001f)
            {
                return targetPosition;
            }

            Vector2 unitDirection = direction / distance;

            float step = movementSpeed * deltaTime;

            if (step > distance)
            {
                step = distance;
            }
            Vector2 result = currentPosition + unitDirection * step;
            return result;
        }

        private readonly List<Movement> m_movements;
    }
}
