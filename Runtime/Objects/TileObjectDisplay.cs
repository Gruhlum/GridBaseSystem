using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class TileObjectDisplay : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer sr = default;

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

            transform.position = tileObject.GetWorldPosition();
            sr.sprite = tileObject.Sprite;
            sr.color = tileObject.Color;
        }

        private void TileObject_OnColorChanged(TileObject arg1, Color color)
        {
            sr.color = color;
        }

        private void TileObject_OnSpriteChanged(TileObject arg1, Sprite sprite)
        {
            sr.sprite = sprite;
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

        private void TileObject_OnRemoved(TileObject obj)
        {
            UnsubscribeEvents();
            visualizer.RemoveDisplay(this);
        }

        private void TileObject_OnMoved(TileObject obj)
        {
            transform.position = obj.GetWorldPosition();
        }
    }
}