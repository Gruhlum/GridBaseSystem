using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class TileObjectVisual2D<T, D, V, S> : TileObjectVisual<T, D, V, S>
       where T : TileObject<T, D, V, S> where D : TileObjectData<T, D, V, S> where V : TileObjectVisual<T, D, V, S> where S : TileObjectSaveData<T, D, V, S>
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

        public override void Setup(T tileObject, BaseGrid grid)
        {
            base.Setup(tileObject, grid);
            if (tileObject != null)
            {
                sr.color = tileObject.Color;
            }
        }
        public override void SetColor(Color color)
        {
            sr.color = color;
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