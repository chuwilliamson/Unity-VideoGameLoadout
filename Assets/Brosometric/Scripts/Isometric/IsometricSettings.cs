namespace Isometric
{
    using UnityEngine;

    [CreateAssetMenu]
    public class IsometricSettings : ScriptableObject
    {
        public Vector2 tileSize = new Vector2(0.64f, 0.32f);
        public float layerSize = 100f;
    }
}
