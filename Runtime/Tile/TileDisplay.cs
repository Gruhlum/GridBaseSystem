using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class TileDisplay : MonoBehaviour
	{
        [SerializeField] protected SpriteRenderer sr = default;

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
            if (Tile != null)
            {
                Tile.OnSpriteChanged -= Tile_OnSpriteChanged;
                tile.OnColorChanged -= Tile_OnColorChanged;
            }
            this.Tile = tile;
            sr.sprite = tile.Sprite;
            sr.color = tile.Color;
            name = tile.ToString();
            tile.OnSpriteChanged += Tile_OnSpriteChanged;
            tile.OnColorChanged += Tile_OnColorChanged;
            tile.OnRemoved += Tile_OnRemoved;
            transform.position = tile.GetWorldPosition();
		}

        private void Tile_OnRemoved(GridObject gridObj)
        {
            gridObj.OnRemoved -= Tile_OnRemoved;
            Tile = null;
            gameObject.SetActive(false);
        }

        private void Tile_OnColorChanged(GridObject arg1, Color color)
        {
            sr.color = color;
        }

        private void Tile_OnSpriteChanged(GridObject arg1, Sprite sprite)
        {
            sr.sprite = sprite;
        }
    }
}