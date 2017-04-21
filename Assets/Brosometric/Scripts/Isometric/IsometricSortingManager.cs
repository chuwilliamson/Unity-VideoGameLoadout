namespace Isometric
{
    using System.Linq;

    using UnityEngine;

    [ExecuteInEditMode]
    [RequireComponent(typeof(IsometricTransform))]
    public class IsometricSortingManager : MonoBehaviour
    {
        [SerializeField]
        private Renderer[] m_Renderers;

        private IsometricTransform m_IsometricTransform;

        private float m_HeightModifier = 1f + Mathf.Sin(Mathf.PI / 6f);

        private void Awake()
        {
            m_IsometricTransform = GetComponent<IsometricTransform>();
        }

        private void LateUpdate()
        {
            if (!m_Renderers.Any())
                return;

            foreach (var renderer in m_Renderers)
                SetSortingOrder(renderer);
        }

        private void SetSortingOrder(Renderer renderer)
        {
            var position = m_IsometricTransform.transform.position.y;
            var height = m_IsometricTransform.position.y * m_IsometricTransform.isometricSettings.tileSize.y;

            var layerSize = m_IsometricTransform.isometricSettings.layerSize;

            renderer.sortingOrder = (int)(-(position - height * m_HeightModifier) * layerSize);
        }
    }
}
