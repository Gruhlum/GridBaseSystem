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
            tile.OnRemoved += Tile_OnRemoved;
            transform.position = tile.GetWorldPosition();
        }

        private void Tile_OnRemoved(GridObject gridObj)
        {
            Deactivate();
        }

        protected virtual void Deactivate()
        {
            Tile.OnRemoved -= Tile_OnRemoved;
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