namespace Isometric
{
    using System;

    using UnityEngine;

    public enum TileStyle
    {
        Grass,
        Stone,
    }

    public enum IsometricDirection
    {
        FrontCenter,
        FrontLeft,
        FrontRight,
        MiddleCenter,
        MiddleLeft,
        MiddleRight,
        BackCenter,
        BackLeft,
        BackRight,
    }

    /// <summary>
    /// This class makes me wish Unity could serialize Dictionaries...
    /// </summary>
    [CreateAssetMenu]
    public class IsometricTileSet : ScriptableObject
    {
        [Serializable]
        public class IsometricSprite
        {
            [HideInInspector]
            public string name;

            public IsometricDirection direction;
            public Sprite sprite;
        }

        [Serializable]
        public class TileSet
        {
            [HideInInspector]
            public string name;

            public TileStyle style;

            [Space]
            public IsometricSprite[] isometricSprites =
                {
                    new IsometricSprite { direction = IsometricDirection.FrontCenter, },
                    new IsometricSprite { direction = IsometricDirection.FrontLeft, },
                    new IsometricSprite { direction = IsometricDirection.FrontRight, },

                    new IsometricSprite { direction = IsometricDirection.MiddleCenter, },
                    new IsometricSprite { direction = IsometricDirection.MiddleLeft, },
                    new IsometricSprite { direction = IsometricDirection.MiddleRight, },

                    new IsometricSprite { direction = IsometricDirection.BackCenter, },
                    new IsometricSprite { direction = IsometricDirection.BackLeft, },
                    new IsometricSprite { direction = IsometricDirection.BackRight, },
                };
        }

        public TileSet[] tileSets =
            {
                new TileSet { style = TileStyle.Grass, },
                new TileSet { style = TileStyle.Stone, },
            };

        private void OnValidate() { UpdateNames(); }

        private void Awake() { UpdateNames(); }

        private void UpdateNames()
        {
            foreach (var tileSet in tileSets)
            {
                tileSet.name = tileSet.style.ToString();
                foreach (var isometricSprite in tileSet.isometricSprites)
                    isometricSprite.name = isometricSprite.direction.ToString();
            }
        }
    }
}
