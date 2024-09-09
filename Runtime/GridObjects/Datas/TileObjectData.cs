using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(menuName = "HexTecGames/Grid/TileObjectData")]
    public class TileObjectData : GridObjectData
    {
        [SerializeField] private List<PlacementCoord> coords = default;
        [SerializeField] protected List<SpriteData> spriteDatas = new List<SpriteData>();

        public TileObjectVisual VisualPrefab
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
        [SerializeField] private TileObjectVisual visualPrefab;
      
        public override bool IsDraggable
        {
            get
            {
                return isDraggable;
            }
        }
        [SerializeField] private bool isDraggable;

        public int TotalSprites
        {
            get
            {
                return spriteDatas.Count;
            }
        }

        public override Sprite Sprite
        {
            get
            {
                return spriteDatas[0].sprite;
            }
        }

        public List<PlacementCoord> GetCoords()
        {
            return new List<PlacementCoord>(coords);
        }

        public List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord center, int rotation)
        {
            List<BoolCoord> boolCoords = new List<BoolCoord>();
            foreach (var coord in coords)
            {
                center.NormalizedAndRotated(coord.coord, rotation);
                boolCoords.Add(new BoolCoord(center, IsValidPlacement(center, grid, rotation)));
            }
            return boolCoords;
        }
        public bool IsValidPlacement(Coord center, BaseGrid grid, int rotation)
        {
            foreach (var coord in coords)
            {
                center.NormalizedAndRotated(coord.coord, rotation);
                var tile = grid.GetTile(center);
                if (tile == null)
                {
                    return false;
                }
                if (!tile.IsBuildable)
                {
                    Debug.Log("Not Buildable!");
                    return false;
                }
                if (coord.type == CoordType.Blocking && !tile.IsPassable)
                {
                    Debug.Log(" 2 2 2 Not Buildable!");
                    return false;
                }
            }
            return true;
        }

        public virtual SpriteData GetSpriteData(Coord center, BaseGrid grid, int rotation)
        {
            return spriteDatas[rotation % spriteDatas.Count];
        }
        public virtual Sprite GetSprite(Coord center, BaseGrid grid, int rotation)
        {
            return spriteDatas[rotation % spriteDatas.Count].sprite;
        }

        public override GridObject CreateObject(Coord center, BaseGrid grid)
        {
            return new TileObject(center, grid, this, 0);
        }
        //public List<PlacementCoord> GetNormalizedCoords(BaseGrid grid, Coord center, int rotation = 0)
        //{
        //    return GetNormalizedCoords(grid, center, coords, rotation);
        //}
        //public List<PlacementCoord> GetCoords()
        //{
        //    return new List<PlacementCoord>(coords);
        //}
        //private List<PlacementCoord> GetNormalizedCoords(BaseGrid grid, Coord center, List<PlacementCoord> coords, int rotation)
        //{
        //    var results = center.GetNormalizedCoords(coords);
        //    if (rotation != 0)
        //    {
        //        results = grid.GetRotatedCoords(center, results, rotation);
        //    }
        //    return results;
        //}
    }
}