using System;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectVisual : MonoBehaviour
    {
        public abstract int TotalRotations { get; }

        public GridObject GridObject
        {
            get
            {
                return gridObject;
            }
            private set
            {
                gridObject = value;
            }
        }
        private GridObject gridObject;

        protected BaseGrid grid;

        public event Action<GridObjectVisual> OnDeactivated;

        public void Setup(GridObject gridObject, BaseGrid grid)
        {
            OnSetup(gridObject, grid);
            gameObject.SetActive(true);
            AfterSetup();
        }
        protected virtual void AfterSetup() { }
        protected virtual void OnSetup(GridObject gridObject, BaseGrid grid)
        {
            if (grid != null)
            {
                this.name = $"{gridObject.Name}Visual {gridObject.Center}";
            }
            this.grid = grid;
            this.gridObject = gridObject;
            Rotate(gridObject.Rotation);
        }
        public abstract void Rotate(int rotation);
        public abstract GridObject GetTileObject();
        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
            OnDeactivated?.Invoke(this);
        }
        public abstract void MoveToFront();
        public abstract void SetColor(Color color);
    }
}