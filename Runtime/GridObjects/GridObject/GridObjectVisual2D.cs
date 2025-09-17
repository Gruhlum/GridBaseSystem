using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectVisual2D<T, D, V> : GridObjectVisual<T, D, V>
       where T : GridObject<T, D, V> where D : GridObjectData<T, D, V> where V : GridObjectVisual2D<T, D, V>
    {
        [SerializeField] protected SpriteRenderer sr = default;
        [SerializeField] private bool useDataColor = true;

        protected virtual void Reset()
        {
            sr = GetComponent<SpriteRenderer>();

            if (sr == null)
            {
                sr = gameObject.AddComponent<SpriteRenderer>();
            }
        }

        protected override void OnSetup(T gridObj, BaseGrid grid)
        {
            base.OnSetup(gridObj, grid);

            if (gridObj != null)
            {
                if (useDataColor)
                {
                    sr.color = gridObj.Color;
                }
                sr.sortingOrder = gridObj.BaseData.Layer;
            }
            else sr.sortingOrder = 5000;
        }
        public override void SetColor(Color color)
        {
            sr.color = color;
        }
        public override void Rotate(int rotation)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, grid.DirectionToDegrees(rotation));
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