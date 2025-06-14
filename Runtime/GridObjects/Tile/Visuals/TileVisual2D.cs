using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class TileVisual2D : TileVisual
    {
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                return sr;
            }
            private set
            {
                sr = value;
            }
        }
        [SerializeField] protected SpriteRenderer sr = default;

        protected virtual void Reset()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();

            if (sr == null)
            {
                sr = gameObject.AddComponent<SpriteRenderer>();
            }
        }

        public override void Setup(Tile tile)
        {
            SetColor(tile.Color);
            base.Setup(tile);
        }

        public override void SetColor(Color color)
        {
            SpriteRenderer.color = color;
        }

        public override void MoveToFront()
        {
            SpriteRenderer.sortingOrder++;
        }
    }
}