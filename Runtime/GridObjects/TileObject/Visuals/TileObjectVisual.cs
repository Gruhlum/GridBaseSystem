using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class TileObjectVisual : MonoBehaviour
    {

        //[SerializeField] protected TileHighlightSpawner highlightSpawner = default;
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
            UnsubscribeEvents();
            this.grid = grid;
            this.visualizer = visualizer;
            this.tileObject = tileObject;
            SubscribeToEvents();

            SetPosition(tileObject);
        }

        protected abstract void Rotate(int rotation);

        private void TileObject_OnRotated(TileObject obj, int rotation)
        {
            Rotate(rotation);
        }

        private void TileObject_OnRemoved(GridObject obj)
        {
            UnsubscribeEvents();
            visualizer.RemoveDisplay(this);
            gameObject.SetActive(false);
        }
        
        protected virtual void TileObject_OnMoved(GridObject obj)
        {
            SetPosition(obj as TileObject);
        }
        protected void SetPosition(TileObject obj)
        {
            transform.position = obj.GetWorldPosition();
            //SpriteData spriteData = obj.GetSpriteData();
            //if (spriteData != null)
            //{
            //    sr.transform.localPosition = spriteData.offset;
            //    sr.transform.eulerAngles = spriteData.rotation;
            //}
        }
        protected virtual void SubscribeToEvents()
        {
            if (tileObject != null)
            {
                tileObject.OnRemoved += TileObject_OnRemoved;
                tileObject.OnMoved += TileObject_OnMoved;
                tileObject.OnRotated += TileObject_OnRotated;
            }          
        }
        protected virtual void UnsubscribeEvents()
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