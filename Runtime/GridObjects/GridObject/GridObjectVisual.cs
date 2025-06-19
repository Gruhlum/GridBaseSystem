using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectVisual<T, D, V, S> : GridObjectVisualBase, ISpawnable<V>
        where T : GridObject<T, D, V, S> where D : GridObjectData<T, D, V, S> where V : GridObjectVisual<T, D, V, S> where S : GridObjectSaveData<T, D, V, S>
    {
        public T TileObject
        {
            get
            {
                return tileObject;
            }
            private set
            {
                tileObject = value;
            }
        }
        private T tileObject;

        protected BaseGrid grid;

        public event Action<V> OnDeactivated;

        protected void OnDisable()
        {
            //Debug.Log("Deactivated");
            OnDeactivated?.Invoke(this as V);
        }

        public virtual void Setup(T tileObject, BaseGrid grid)
        {
            if (TileObject != null)
            {
                RemoveEvents(TileObject);
            }
            this.name = $"{tileObject.Name}Visual {tileObject.Center}";
            this.grid = grid;
            this.tileObject = tileObject;

            if (tileObject != null)
            {
                AddEvents(tileObject);
                SetPosition(tileObject);
            }
        }

        protected abstract void Rotate(int rotation);

        private void TileObject_OnRotated(T obj, int rotation)
        {
            Rotate(rotation);
        }

        protected virtual void TileObject_OnRemoved(T obj)
        {
            if (TileObject != null)
            {
                RemoveEvents(TileObject);
            }
            Deactivate();
        }
        protected virtual void TileObject_OnMoved(T obj, Coord old, Coord current)
        {
            SetPosition(obj);
        }
        protected virtual void SetPosition(T obj)
        {
            transform.position = obj.GetWorldPosition();
        }
        protected virtual void AddEvents(T tileObject)
        {
            tileObject.OnRemoved += TileObject_OnRemoved;
            tileObject.OnMoved += TileObject_OnMoved;
            tileObject.OnRotated += TileObject_OnRotated;
        }
        protected virtual void RemoveEvents(T tileObject)
        {
            tileObject.OnRemoved -= TileObject_OnRemoved;
            tileObject.OnMoved -= TileObject_OnMoved;
            tileObject.OnRotated -= TileObject_OnRotated;
        }

        public override GridObjectBase GetTileObject()
        {
            return TileObject;
        }
    }
}