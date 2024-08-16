using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class TileObjectDisplay : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer sr = default;

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
        private TileObjectVisualizer visualizer;

        private void Reset()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public virtual void Setup(TileObject tileObject, TileObjectVisualizer visualizer)
        {
            UnsubscribeEvents();

            this.visualizer = visualizer;
            this.tileObject = tileObject;
            tileObject.OnRemoved += TileObject_OnRemoved;
            tileObject.OnMoved += TileObject_OnMoved;
            tileObject.OnSpriteChanged += TileObject_OnSpriteChanged;
            tileObject.OnColorChanged += TileObject_OnColorChanged;

            SetPosition(tileObject);
            sr.sprite = tileObject.Sprite;
            sr.color = tileObject.Color;
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

        private void TileObject_OnMoved(GridObject obj)
        {
            SetPosition(obj);
        }
        private void SetPosition(GridObject obj)
        {
            transform.position = obj.GetWorldPosition();
            transform.eulerAngles = new Vector3(0, 0, obj.Rotation * -90);
        }
        private void UnsubscribeEvents()
        {
            if (tileObject != null)
            {
                tileObject.OnRemoved -= TileObject_OnRemoved;
                tileObject.OnMoved -= TileObject_OnMoved;
                tileObject.OnSpriteChanged -= TileObject_OnSpriteChanged;
                tileObject.OnColorChanged -= TileObject_OnColorChanged;
            }
        }
    }
}