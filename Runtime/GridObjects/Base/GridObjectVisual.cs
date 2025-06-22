using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectVisual : MonoBehaviour
    {
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

        public virtual void Setup(GridObject gridObject, BaseGrid grid)
        {
            if (grid != null)
            {
                this.name = $"{gridObject.Name}Visual {gridObject.Center}";
            }
            this.grid = grid;
            this.gridObject = gridObject;
        }

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