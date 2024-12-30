using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[CreateAssetMenu(menuName = "HexTecGames/Grid/TileData")]
	public class TileData : GridObjectData
    {
        public TileVisual VisualPrefab
        {
            get
            {
                return visualPrefab;
            }
            private set
            {
                visualPrefab = value;
            }
        }
        [SerializeField] private TileVisual visualPrefab;

        public virtual Tile CreateObject(BaseGrid grid, Coord center)
        {
            return new Tile(grid, this, center);
        }

        public bool IsValidCoord(Coord coord, BaseGrid grid)
        {
            if (!grid.DoesTileExist(coord))
            {
                return true;
            }
            return false;
        }
    }
}