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

        public override Sprite Sprite
        {
            get
            {
                return sprite;
            }
        }
        [SerializeField] private Sprite sprite = default;

        public override GridObject CreateObject(Coord center, BaseGrid grid)
        {
            return new Tile(center, grid, this);
        }

        public bool IsValidCoord(Coord coord, BaseGrid grid)
        {
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