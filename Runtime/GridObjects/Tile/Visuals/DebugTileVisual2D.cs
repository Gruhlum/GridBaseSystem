using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class DebugTileVisual2D : TileVisual2D
    {
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color passableColor = Color.yellow;
        [SerializeField] private Color blockedColor = Color.red;

        public override void Setup(Tile tile)
        {
            base.Setup(tile);
            tile.OnTileObjectAdded += Tile_OnTileObjectAdded;
            tile.OnTileObjectRemoved += Tile_OnTileObjectRemoved;
            SetColor(normalColor);
        }

        protected override void Deactivate()
        {
            Tile.OnTileObjectAdded -= Tile_OnTileObjectAdded;
            Tile.OnTileObjectRemoved -= Tile_OnTileObjectRemoved;
            base.Deactivate();
        }

        private void SetColor(Tile tile)
        {
            if (tile.IsEmpty)
            {
                SetColor(normalColor);
            }
            else if (tile.IsPassable)
            {
                SetColor(passableColor);
            }
            else SetColor(blockedColor);

        }

        private void Tile_OnTileObjectAdded(Tile tile, TileObjectPlacement data)
        {
            SetColor(tile);
        }

        private void Tile_OnTileObjectRemoved(Tile tile, TileObjectPlacement data)
        {
            SetColor(tile);
        }
    }
}