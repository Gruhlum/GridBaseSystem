using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[CreateAssetMenu(menuName = "HexTecGames/Grid/TileData")]
	public class TileData : GridObjectData
    {
        public override bool IsDraggable
        {
            get
            {
                return true;
            }
        }

        public override bool IsReplaceable
        {
            get
            {
                return isReplaceable;
            }
        }
        [SerializeField] private bool isReplaceable = default;

        public override GridObject CreateObject(Coord center, BaseGrid grid)
        {
            return new Tile(center, grid, this);
        }

        public override bool IsValidPlacement(Coord coord, BaseGrid grid, int rotation)
        {
            return IsValidCoord(coord, grid);
        }

        public override bool IsValidCoord(Coord coord, BaseGrid grid)
        {
            if (!grid.IsAllowedCoord(coord))
            {
                return false;
            }
            if (!grid.DoesTileExist(coord))
            {
                return true;
            }
            if (grid.GetTile(coord).Data.IsReplaceable)
            {
                return true;
            }
            return false;
        }
    }
}