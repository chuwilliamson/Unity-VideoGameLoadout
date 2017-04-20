namespace Isometric.Mask
{
    using System.Collections.Generic;
    using System.Linq;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    using UnityEngine;

    [ExecuteInEditMode]
    [RequireComponent(typeof(IsometricTransform))]
    public abstract class IsometricMask : MonoBehaviour
    {
        [SerializeField]
        private MaskType m_MaskType;

        [SerializeField]
        private IsometricDirection m_MaskDirection;

        [Space, SerializeField]
        private AnimationCurve m_LateralTopography = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        [SerializeField]
        private AnimationCurve m_LongitudinalTopography = AnimationCurve.Linear(0f, 1f, 1f, 1f);

        private IsometricTransform m_IsometricTransform;

        public MaskType maskType { get { return m_MaskType; } }
        public IsometricDirection maskDirection { get { return m_MaskDirection; } }

        public AnimationCurve lateralTopography { get { return m_LateralTopography; } }
        public AnimationCurve longitudinalTopography { get { return m_LongitudinalTopography; } }

        public IsometricTransform isometricTransform { get { return m_IsometricTransform; } }

        protected virtual void Awake() { m_IsometricTransform = GetComponent<IsometricTransform>(); }
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(IsometricMask))]
    public class IsometricMaskRenderer : MonoBehaviour
    {
        [SerializeField]
        private IsometricMaskSettings m_IsometricMaskSettings;

        private MeshRenderer m_MeshRenderer;
        private MeshFilter m_MeshFilter;
        private IsometricMask m_IsometricMask;

#if UNITY_EDITOR
        private void OnRenderObject()
        {
            if (m_MeshRenderer == null)
                return;

            UpdateMesh();
        }

        private void Awake()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_MeshFilter = GetComponent<MeshFilter>();
            m_IsometricMask = GetComponent<IsometricMask>();
        }

        private void UpdateMesh()
        {
            var mesh = new Mesh { subMeshCount = 2, };

            var lateralStart = m_IsometricMask.lateralTopography.Evaluate(0f);
            var lateralEnd = m_IsometricMask.lateralTopography.Evaluate(1f);

            var longitudinalStart = m_IsometricMask.longitudinalTopography.Evaluate(0f);
            var longitudinalEnd = m_IsometricMask.longitudinalTopography.Evaluate(1f);

            var vertexes =
                new List<Vector3>
                {
                    new Vector3(
                        0.5f,
                        Mathf.Max(
                            m_IsometricMask.lateralTopography.Evaluate(0.5f),
                            m_IsometricMask.longitudinalTopography.Evaluate(0.5f)),
                        -0.5f),
                    new Vector3(0.5f, m_IsometricMask.lateralTopography.Evaluate(1f), 0f),
                    new Vector3(0f, m_IsometricMask.longitudinalTopography.Evaluate(0f), -0.5f),
                    new Vector3(0f, Mathf.Round((lateralEnd + longitudinalStart) / 2f), 0f),

                    new Vector3(
                        0.5f,
                        Mathf.Max(
                            m_IsometricMask.lateralTopography.Evaluate(0.5f),
                            m_IsometricMask.longitudinalTopography.Evaluate(0.5f)),
                        -0.5f),
                    new Vector3(0.5f, m_IsometricMask.lateralTopography.Evaluate(0f), -1f),
                    new Vector3( 0f, m_IsometricMask.longitudinalTopography.Evaluate(0f), -0.5f),
                    new Vector3(0f, Mathf.Round((lateralStart + longitudinalStart) / 2f), -1f),

                    new Vector3(
                        0.5f,
                        Mathf.Max(
                            m_IsometricMask.lateralTopography.Evaluate(0.5f),
                            m_IsometricMask.longitudinalTopography.Evaluate(0.5f)),
                        -0.5f),
                    new Vector3(0.5f, m_IsometricMask.lateralTopography.Evaluate(0f), -1f),
                    new Vector3(1f, m_IsometricMask.longitudinalTopography.Evaluate(1f), -0.5f),
                    new Vector3(1f, Mathf.Round((lateralStart + longitudinalEnd) / 2f), -1f),

                    new Vector3(
                        0.5f,
                        Mathf.Max(
                            m_IsometricMask.lateralTopography.Evaluate(0.5f),
                            m_IsometricMask.longitudinalTopography.Evaluate(0.5f)),
                        -0.5f),
                    new Vector3(0.5f, m_IsometricMask.lateralTopography.Evaluate(1f), 0f),
                    new Vector3(1f, m_IsometricMask.longitudinalTopography.Evaluate(1f), -0.5f),
                    new Vector3(1f, Mathf.Round((lateralEnd + longitudinalEnd) / 2f), 0f),
                };

            var previousValue = 0f;
            for (var i = 0f; i <= 1.1f; i += 0.1f)
            {
                var lineStart =
                    new Vector3(
                        0.5f,
                        m_IsometricMask.lateralTopography.Evaluate(previousValue),
                        -1f + previousValue);

                var lineEnd =
                    new Vector3(0.5f, m_IsometricMask.lateralTopography.Evaluate(i), -1f + i);

                vertexes.Add(lineStart);
                vertexes.Add(lineEnd);

                previousValue = i;
            }

            previousValue = 0f;
            for (var i = 0f; i <= 1.1f; i += 0.1f)
            {
                var lineStart =
                    new Vector3(
                        previousValue,
                        m_IsometricMask.longitudinalTopography.Evaluate(previousValue),
                        -0.5f);

                var lineEnd =
                    new Vector3(i, m_IsometricMask.longitudinalTopography.Evaluate(i), -0.5f);

                vertexes.Add(lineStart);
                vertexes.Add(lineEnd);

                previousValue = i;
            }

            var drawIn3D = SceneView.lastActiveSceneView == null || !SceneView.lastActiveSceneView.in2DMode;
            for (var i = 0; i < vertexes.Count; ++i)
            {
                if (!drawIn3D)
                {
                    vertexes[i] = m_IsometricMask.isometricTransform.ToCartesian(vertexes[i]);
                    vertexes[i] += -Vector3.forward;
                }

                if (!drawIn3D)
                    continue;

                vertexes[i] = new Vector3(vertexes[i].x, vertexes[i].y, -vertexes[i].z);
                vertexes[i] += -Vector3.forward * 8f;
                vertexes[i] +=
                    new Vector3(
                        m_IsometricMask.isometricTransform.position.x,
                        m_IsometricMask.isometricTransform.position.y,
                        -m_IsometricMask.isometricTransform.position.z);
                vertexes[i] -= m_IsometricMask.transform.localPosition;
            }

            var indexes =
                new[]
                {
                    0, 1, 2,
                    1, 3, 2,

                    6, 5, 4,
                    6, 7, 5,

                    8, 9, 10,
                    9, 11, 10,

                    14, 13, 12,
                    14, 15, 13,
                };

            mesh.SetVertices(vertexes);
            mesh.SetIndices(indexes, MeshTopology.Triangles, 0);

            mesh.RecalculateNormals();

            indexes = new int[vertexes.Count - 16];
            for (var i = 0; i < indexes.Length; ++i)
                indexes[i] = i + 16;

            mesh.SetIndices(indexes, MeshTopology.Lines, 1);

            m_MeshFilter.mesh = mesh;

            var maskMaterial =
                m_IsometricMaskSettings.isometricMaskMaterials.FirstOrDefault(
                    material => material.type == m_IsometricMask.maskType);
            if (maskMaterial == null)
                return;

            m_MeshRenderer.material = maskMaterial.material;
        }
#endif

    }
}
