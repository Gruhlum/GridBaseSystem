using HexTecGames.Basics;
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

        public override bool IsValidCoord(BaseGrid grid, Coord coord, int rotation = 0)
        {
            return !grid.DoesTileExist(coord);
        }

        public override List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord center, int rotation)
        {
            if (IsValidCoord(grid, center, rotation))
            {
                return new List<BoolCoord>() { new BoolCoord(center, true) };
            }
            else return new List<BoolCoord>() { new BoolCoord(center, false) };
        }

        public override GridObjectVisual GetVisual()
        {
            return VisualPrefab;
        }
    }
}