using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    //[CreateAssetMenu(menuName = "HexTecGames/MultiTileObjectData")]
    public abstract class MultiTileObjectData : BaseTileObjectData
    {
        [SerializeField] private List<Coord> coords = default;

        public override List<Coord> GetCoords()
        {
            return new List<Coord>(coords);
        }

        public override bool IsValidCoord(Coord center, BaseGrid grid, int rotation)
        {
            foreach (var coord in grid.GetRotatedCoords(center, center.GetNormalizedCoords(coords), rotation))
            {
                if (!grid.IsTileEmpty(coord))
                {
                    return false;
                }
            }          
            return true;
        }
        public override Sprite GetSprite()
        {
            return sprites[0];
        }
        public override bool IsDraggable
        {
            get
            {
                return false;
            }
        }
    }
}