using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class TileObjectVisual : MonoBehaviour
	{
		[SerializeField] protected SpriteRenderer sr = default;
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

        private bool showSafetyZones = true;

        protected BaseGrid grid;

        protected void Reset()
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr == null)
            {
               sr = GetComponent<SpriteRenderer>();
            }
            if (sr == null)
            {
                sr = gameObject.AddComponent<SpriteRenderer>();
            }
        }

        public virtual void Setup(TileObject tileObject, TileObjectVisualizer visualizer, BaseGrid grid)
        {
            UnsubscribeEvents();
            this.grid = grid;
            this.visualizer = visualizer;
            this.tileObject = tileObject;
            tileObject.OnRemoved += TileObject_OnRemoved;
            tileObject.OnMoved += TileObject_OnMoved;
            tileObject.OnSpriteChanged += TileObject_OnSpriteChanged;
            tileObject.OnColorChanged += TileObject_OnColorChanged;

            SetPosition(tileObject);
            sr.sprite = tileObject.Sprite;
            sr.color = tileObject.Color;
            //ActivateSafetyZoneHighlight(showSafetyZones);
        }

        private void TileObject_OnColorChanged(GridObject arg1, Color color)
        {
            sr.color = color;
        }

        private void TileObject_OnSpriteChanged(GridObject arg1, Sprite sprite)
        {
            sr.sprite = sprite;
        }
        private void TileObject_OnRemoved(GridObject obj)
        {
            UnsubscribeEvents();
            visualizer.RemoveDisplay(this);
            gameObject.SetActive(false);
        }

        protected virtual void TileObject_OnMoved(GridObject obj)
        {
            SetPosition(obj);
        }
        protected void SetPosition(GridObject obj)
        {
            transform.position = obj.GetWorldPosition();
            SpriteData spriteData = obj.GetSpriteData();
            sr.transform.localPosition = spriteData.offset;
            sr.transform.eulerAngles = spriteData.rotation;
        }
        protected void UnsubscribeEvents()
        {
            if (tileObject != null)
            {
                tileObject.OnRemoved -= TileObject_OnRemoved;
                tileObject.OnMoved -= TileObject_OnMoved;
                tileObject.OnSpriteChanged -= TileObject_OnSpriteChanged;
                tileObject.OnColorChanged -= TileObject_OnColorChanged;
            }
        }

        public void ActivateSafetyZoneHighlight(bool active)
        {
            //if (active)
            //{
            //    highlightSpawner.SpawnHighlights(TileObject.GetNormalizedSafeZones());
            //}
            //else highlightSpawner.DeactivateAll();
        }
    }
}