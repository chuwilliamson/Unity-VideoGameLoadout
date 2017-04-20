namespace Isometric.Mask
{
    using System.Linq;

    using Library;

    using UnityEngine;

    [ExecuteInEditMode]
    public class IsometricMaskManager : MonoSingleton<IsometricMaskManager>
    {
        private IsometricMask[,] m_IsometricMasks;
        private Vector3 m_IndexOffset;

        protected override void OnAwake()
        {
            var gameObjects = FindObjectsOfType<IsometricMask>().Select(mask => mask.gameObject);

            var largestValue = Vector3.zero;
            var smallestValue = Vector3.zero;
            foreach (var gameObject in gameObjects)
            {
                var mask = gameObject.GetComponent<IsometricMask>();
                var isometricTransform = gameObject.GetComponent<IsometricTransform>();
                if (mask == null || isometricTransform == null)
                    continue;

                var isometricPosition = isometricTransform.position;
                if (isometricPosition.x > largestValue.x)
                    largestValue.x = isometricPosition.x;
                if (isometricPosition.x < smallestValue.x)
                    smallestValue.x = isometricPosition.x;

                if (isometricPosition.y > largestValue.y)
                    largestValue.y = isometricPosition.x;
                if (isometricPosition.y < smallestValue.y)
                    smallestValue.y = isometricPosition.y;

                if (isometricPosition.z > largestValue.z)
                    largestValue.z = isometricPosition.z;
                if (isometricPosition.z < smallestValue.z)
                    smallestValue.z = isometricPosition.z;
            }

            m_IndexOffset = smallestValue;

            var sizeX = (int)Mathf.Abs(largestValue.x - smallestValue.x);
            var sizeZ = (int)Mathf.Abs(largestValue.z - smallestValue.z);

            m_IsometricMasks = new IsometricMask[sizeX + 1, sizeZ + 1];
            foreach (var gameObject in gameObjects)
            {
                var mask = gameObject.GetComponent<IsometricMask>();
                var isometricTransform = gameObject.GetComponent<IsometricTransform>();
                if (mask == null || isometricTransform == null)
                    continue;

                var positionX = (int)(isometricTransform.position.x - m_IndexOffset.x);
                var positionZ = (int)(isometricTransform.position.z - m_IndexOffset.z);
                m_IsometricMasks[positionX, positionZ] = mask;
            }
        }

        public IsometricMask GetMask(int x, int z)
        {
            x -= (int)m_IndexOffset.x;
            z -= (int)m_IndexOffset.z;

            if (x < 0 || x >= m_IsometricMasks.GetLength(0))
                return null;
            if (z < 0 || z >= m_IsometricMasks.GetLength(1))
                return null;

            return m_IsometricMasks[x, z];
        }

        public IsometricMask GetMask(float x, float z)
        {
            return GetMask((int)x, (int)z);
        }
    }
}
