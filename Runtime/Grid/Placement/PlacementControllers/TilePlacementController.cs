using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class TilePlacementController : GridPlacementController
    {
        public TileData SelectedTileData
        {
            get
            {
                return selectedTileData;
            }
            private set
            {
                if (selectedTileData == value)
                {
                    return;
                }
                selectedTileData = value;
            }
        }
        private TileData selectedTileData;

        public override GridObjectData SelectedObject
        {
            get
            {
                return SelectedTileData;
            }
        }

        protected override bool IsValidCoord(Coord coord)
        {
            return SelectedTileData.IsValidCoord(coord, grid);
        }

        protected override GridObject GenerateObject(Coord coord)
        {
            Tile tile = SelectedTileData.CreateObject(coord, grid) as Tile;
            grid.AddTile(tile);
            return tile;
        }
        public void SetSelectedObject(TileData data)
        {
            if (data == null)
            {
                ClearSelectedObject();
                return;
            }

            SelectedTileData = data;
            ghost.Activate(data, HoverCoord);
        }
        protected override void ClearSelected()
        {
            SelectedTileData = null;
        }
    }
}