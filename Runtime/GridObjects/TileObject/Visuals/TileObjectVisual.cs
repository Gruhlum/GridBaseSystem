using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class TileObjectVisual : MonoBehaviour
    {
        public TileObject TileObject
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
        private TileObject tileObject;
        protected TileObjectVisualizer visualizer;

        protected BaseGrid grid;


        public virtual void Setup(TileObject tileObject, TileObjectVisualizer visualizer, BaseGrid grid)
        {
            if (TileObject != null)
            {
                RemoveEvents();
            }
            this.grid = grid;
            this.visualizer = visualizer;
            this.tileObject = tileObject;
            AddEvents();

            SetPosition(tileObject);
        }

        protected abstract void Rotate(int rotation);

        private void TileObject_OnRotated(TileObject obj, int rotation)
        {
            Rotate(rotation);
        }

        protected virtual void TileObject_OnRemoved(GridObject obj)
        {
            RemoveEvents();
            Deactivate();
        }
        protected void Deactivate()
        {
            visualizer.RemoveDisplay(this);
            gameObject.SetActive(false);
        }
        protected virtual void TileObject_OnMoved(GridObject obj)
        {
            SetPosition(obj as TileObject);
        }
        protected virtual void SetPosition(TileObject obj)
        {
            transform.position = obj.GetWorldPosition();
        }
        protected virtual void AddEvents()
        {
            if (tileObject != null)
            {
                tileObject.OnRemoved += TileObject_OnRemoved;
                tileObject.OnMoved += TileObject_OnMoved;
                tileObject.OnRotated += TileObject_OnRotated;
            }          
        }
        protected virtual void RemoveEvents()
        {
            if (tileObject != null)
            {
                tileObject.OnRemoved -= TileObject_OnRemoved;
                tileObject.OnMoved -= TileObject_OnMoved;
                tileObject.OnRotated -= TileObject_OnRotated;
            }
        }
    }
}