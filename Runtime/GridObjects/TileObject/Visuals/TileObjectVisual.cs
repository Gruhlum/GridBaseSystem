using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class TileObjectVisual<T, D, V, S> : TileObjectVisualBase, ISpawnable<V>
        where T : TileObject<T, D, V, S> where D : TileObjectData<T, D, V, S> where V : TileObjectVisual<T, D, V, S> where S : TileObjectSaveData<T, D, V, S>
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
            OnDeactivated?.Invoke(this as V);
        }

        public virtual void Setup(T tileObject, BaseGrid grid)
        {
            if (TileObject != null)
            {
                RemoveEvents(TileObject);
            }
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

        public override GridObject GetTileObject()
        {
            return TileObject;
        }
    }
}