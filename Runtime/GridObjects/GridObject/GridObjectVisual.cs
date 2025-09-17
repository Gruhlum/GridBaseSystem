using System;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectVisual<T, D, V> : GridObjectVisual, ISpawnable<V>
        where T : GridObject<T, D, V> where D : GridObjectData<T, D, V> where V : GridObjectVisual<T, D, V>
    {
        public new T GridObject
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
        private T gridObject;

        public new event Action<V> OnDeactivated;

        protected virtual void OnDisable()
        {
            //Debug.Log("Deactivated");
            OnDeactivated?.Invoke(this as V);
        }

        protected sealed override void OnSetup(GridObject gridObject, BaseGrid grid)
        {
            if (GridObject != null)
            {
                RemoveEvents(GridObject);
            }

            base.OnSetup(gridObject, grid);

            if (gridObject is T t)
            {
                this.GridObject = t;
                OnSetup(t, grid);
            }
            else
            {
                Debug.Log("Wrong Type!");
                return;
            }

            if (GridObject != null)
            {
                AddEvents(GridObject);
                SetPosition(GridObject);
            }
        }

        protected virtual void OnSetup(T gridObj, BaseGrid grid)
        {
        }

        private void GridObject_OnRotated(T gridObj, int rotation)
        {
            Rotate(rotation);
        }

        protected virtual void GridObject_OnRemoved(T gridObj)
        {
            if (gridObj != null)
            {
                RemoveEvents(gridObj);
            }
            Deactivate();
        }
        protected virtual void GridObject_OnMoved(T gridObj, Coord old, Coord current)
        {
            SetPosition(gridObj);
        }

        protected virtual void SetPosition(T gridObj)
        {
            transform.position = gridObj.GetWorldPosition();
        }
        protected virtual void AddEvents(T gridObj)
        {
            gridObj.OnRemoved += GridObject_OnRemoved;
            gridObj.OnMoved += GridObject_OnMoved;
            gridObj.OnRotated += GridObject_OnRotated;
        }

        protected virtual void RemoveEvents(T gridObj)
        {
            gridObj.OnRemoved -= GridObject_OnRemoved;
            gridObj.OnMoved -= GridObject_OnMoved;
            gridObj.OnRotated -= GridObject_OnRotated;
        }

        public override GridObject GetTileObject()
        {
            return GridObject;
        }
    }
}