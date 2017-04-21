namespace Isometric
{
    using UnityEngine;
    using UnityEngine.Events;

    public class IsometricTransform : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_Position;

        [Space, SerializeField]
        private IsometricSettings m_IsometricSettings;

        private UnityEvent m_OnTransformChanged = new UnityEvent();

        public Vector3 position
        {
            get { return m_Position; }
            set { m_Position = value; OnTransformChanged(); }
        }

        public IsometricSettings isometricSettings { get { return m_IsometricSettings; } }

        public UnityEvent onTransformChanged { get { return m_OnTransformChanged; } }

        private void OnValidate()
        {
            OnTransformChanged();
        }

        private void OnTransformChanged()
        {
            transform.localPosition = ToCartesian(m_Position);

            m_OnTransformChanged.Invoke();
        }

        public Vector3 ToCartesian(Vector3 isometricPosition)
        {
            var tileSize = m_IsometricSettings.tileSize;

            return new Vector3(
                isometricPosition.z * tileSize.x / 2f + isometricPosition.x * tileSize.x / 2f,
                isometricPosition.y * tileSize.y
                    + (isometricPosition.x * tileSize.y / 2f - isometricPosition.z * tileSize.y / 2f),
                0f);
        }
    }
}
