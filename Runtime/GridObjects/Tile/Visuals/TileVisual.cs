using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class TileVisual : MonoBehaviour
    {
        public Tile Tile
        {
            get
            {
                return tile;
            }
            private set
            {
                tile = value;
            }
        }
        private Tile tile;


        public virtual void Setup(Tile tile)
        {
            this.Tile = tile;
            name = tile.ToString();
            AddEvents(tile);
            transform.position = tile.GetWorldPosition();
        }

        protected virtual void AddEvents(Tile tile)
        {
            if (tile == null)
            {
                return;
            }
            tile.OnRemoved += Tile_OnRemoved;
            tile.OnColorChanged += Tile_OnColorChanged;
        }
        protected virtual void RemoveEvents(Tile tile)
        {
            if (tile == null)
            {
                return;
            }
            tile.OnRemoved -= Tile_OnRemoved;
            tile.OnColorChanged -= Tile_OnColorChanged;
        }
        private void Tile_OnColorChanged(GridObject obj, Color color)
        {
            SetColor(color);
        }

        public abstract void SetColor(Color color);

        private void Tile_OnRemoved(GridObject gridObj)
        {
            Deactivate();
        }

        protected virtual void Deactivate()
        {
            RemoveEvents(Tile);
            gameObject.SetActive(false);
        }



        //private void Tile_OnColorChanged(GridObject arg1, Color color)
        //{
        //    sr.color = color;
        //}

        //private void Tile_OnSpriteChanged(GridObject arg1, Sprite sprite)
        //{
        //    sr.sprite = sprite;
        //}
    }
}