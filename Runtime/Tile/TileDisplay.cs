using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class TileDisplay : MonoBehaviour
	{
        [SerializeField] private SpriteRenderer sr = default;

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


        public void Setup(Tile tile)
		{
            if (Tile != null)
            {
                Tile.OnSpriteChanged -= Tile_OnSpriteChanged;
                tile.OnColorChanged -= Tile_OnColorChanged;
            }

            this.Tile = tile;
            sr.sprite = tile.Sprite;
            name = tile.ToString();
            tile.OnSpriteChanged += Tile_OnSpriteChanged;
            tile.OnColorChanged += Tile_OnColorChanged;
            transform.position = tile.GetWorldPosition();
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