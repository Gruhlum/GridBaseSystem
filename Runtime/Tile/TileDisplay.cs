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
            this.Tile = tile;
            sr.sprite = tile.Data.GetSprite(tile.Center, tile.Grid);
            name = tile.ToString();
            transform.position = tile.GetWorldPosition();
		}
	}
}