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

        public override PlacementData SelectedObject
        {
            get
            {
                return null; //TODO
            }
        }

        protected override bool IsValidCoord(Coord coord)
        {
            return SelectedTileData.IsValidCoord(coord, grid);
        }

        protected override GridObject GenerateObject(Coord coord)
        {
            Tile tile = SelectedTileData.CreateObject(grid, coord) as Tile;
            grid.AddTile(tile);
            return tile;
        }
        public void SetSelectedObject(PlacementData data)
        {
            if (data == null)
            {
                ClearSelectedObject();
                return;
            }

            //SelectedTileData = data;
            //ghost.Activate(data, HoverCoord);
        }
        protected override void ClearSelected()
        {
            SelectedTileData = null;
        }
    }
}