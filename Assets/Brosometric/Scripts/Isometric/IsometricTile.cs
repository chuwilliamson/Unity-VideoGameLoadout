namespace Isometric
{
    using System.Linq;

    using UnityEngine;

    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class IsometricTile : MonoBehaviour
    {
        [SerializeField]
        private IsometricTileSet m_IsometricTileSet;

        [Space, SerializeField]
        private TileStyle m_Style;
        [SerializeField]
        private IsometricDirection m_Direction;

        private SpriteRenderer m_SpriteRenderer;

        private void OnValidate()
        {
            if (m_SpriteRenderer == null)
                return;

            var tileSet = m_IsometricTileSet.tileSets.FirstOrDefault(set => set.style == m_Style);
            if (tileSet == null)
                return;

            var tileSprite =
                tileSet.isometricSprites.FirstOrDefault(sprite => sprite.direction == m_Direction);
            if (tileSprite == null)
                return;

            m_SpriteRenderer.sprite = tileSprite.sprite;
        }

        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

            OnValidate();
        }
    }
}
