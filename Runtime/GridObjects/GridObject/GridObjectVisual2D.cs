using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectVisual2D<T, D, V, S> : GridObjectVisual<T, D, V, S>
       where T : GridObject<T, D, V, S> where D : GridObjectData<T, D, V, S> where V : GridObjectVisual2D<T, D, V, S> where S : GridObjectSaveData<T, D, V, S>
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

        public override void Setup(T gridObj, BaseGrid grid)
        {
            base.Setup(gridObj, grid);

            sr.sortingOrder = gridObj.BaseData.Layer;

            if (gridObj != null)
            {
                sr.color = gridObj.Color;
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

        protected override void AddEvents(T gridObj)
        {
            base.AddEvents(gridObj);
            gridObj.OnColorChanged += GridObject_OnColorChanged;
        }

        protected override void RemoveEvents(T gridObj)
        {
            base.RemoveEvents(gridObj);
            gridObj.OnColorChanged -= GridObject_OnColorChanged;
        }
        private void GridObject_OnColorChanged(T gridObj, Color color)
        {
            SetColor(color);
        }
        public override void MoveToFront()
        {
            sr.sortingOrder++;
        }
    }
}