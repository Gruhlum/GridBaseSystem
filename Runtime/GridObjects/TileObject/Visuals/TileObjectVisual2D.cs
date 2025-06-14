using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class TileObjectVisual2D : TileObjectVisual
    {
        [SerializeField] protected SpriteRenderer sr = default;

        protected void Reset()
        {
            sr = GetComponent<SpriteRenderer>();

            if (sr == null)
            {
                sr = gameObject.AddComponent<SpriteRenderer>();
            }
        }

        public override void Setup(TileObject tileObject, TileObjectVisualizer visualizer, BaseGrid grid)
        {
            base.Setup(tileObject, visualizer, grid);
            sr.color = tileObject.Color;
        }

        protected override void Rotate(int rotation)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, TileObject.DirectionToDegrees(rotation));
        }

        public override void MoveToFront()
        {
            sr.sortingOrder++;
        }
    }
}