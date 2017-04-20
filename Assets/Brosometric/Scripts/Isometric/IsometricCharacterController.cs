namespace Isometric
{
    using UnityEngine;

    [RequireComponent(typeof(IsometricCharacter))]
    public class IsometricCharacterController : MonoBehaviour
    {
        private IsometricCharacter m_IsometricCharacter;

        private void Awake() { m_IsometricCharacter = GetComponent<IsometricCharacter>(); }

        private void FixedUpdate()
        {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            var direction = Direction.None;

            if (h > 0f)
                direction = Direction.Right;
            else if (h < 0f)
                direction = Direction.Left;

            if (v > 0f)
                direction = Direction.Up;
            else if (v < 0f)
                direction = Direction.Down;

            if (direction == Direction.None)
                return;

            m_IsometricCharacter.Move(direction);
        }
    }
}
