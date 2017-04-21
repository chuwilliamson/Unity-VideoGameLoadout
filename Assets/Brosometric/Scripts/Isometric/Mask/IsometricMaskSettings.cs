namespace Isometric.Mask
{
    using System;

    using UnityEngine;

    public enum MaskType
    {
        Standard,
        Impassable,
        Liquid,
        Hazard,
    }

    [CreateAssetMenu]
    public class IsometricMaskSettings : ScriptableObject
    {
        [Serializable]
        public class IsometricMaskMaterial
        {
            [HideInInspector]
            public string name;

            public MaskType type;
            public Material material;
        }

        [Space]
        public IsometricMaskMaterial[] isometricMaskMaterials =
            {
                new IsometricMaskMaterial { type = MaskType.Standard, },
                new IsometricMaskMaterial { type = MaskType.Impassable, },
                new IsometricMaskMaterial { type = MaskType.Liquid, },
                new IsometricMaskMaterial { type = MaskType.Hazard, },
            };

        private void OnValidate() { UpdateNames(); }

        private void Awake() { UpdateNames(); }

        private void UpdateNames()
        {
            foreach (var isometricMaskColor in isometricMaskMaterials)
                isometricMaskColor.name = isometricMaskColor.type.ToString();
        }
    }
}
