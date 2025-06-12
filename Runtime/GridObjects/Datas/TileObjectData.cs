using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(menuName = "HexTecGames/Grid/TileObjectData")]
    public class TileObjectData : GridObjectData
    {
        [SerializeField] public List<PlacementCoord> coords = new List<PlacementCoord>() { new PlacementCoord() };
        //[SerializeField] protected List<SpriteData> spriteDatas = new List<SpriteData>();


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

        //public int TotalSprites
        //{
        //    get
        //    {
        //        return spriteDatas.Count;
        //    }
        //}

        public override List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord center, int rotation)
        {
            List<BoolCoord> boolCoords = new List<BoolCoord>();
           
            foreach (var placementCoord in coords)
            {
                Coord normalized = center;
                normalized.NormalizeAndRotate(placementCoord.coord, rotation);
                boolCoords.Add(new BoolCoord(normalized, IsTileValid(grid, normalized, placementCoord.type)));
            }
            return boolCoords;
        }

        //public virtual SpriteData GetSpriteData(Coord center, BaseGrid grid, int rotation)
        //{
        //    if (spriteDatas.Count <= 0)
        //    {
        //        return null;
        //    }
        //    return spriteDatas[rotation % spriteDatas.Count];
        //}
        //public virtual Sprite GetSprite(Coord center, BaseGrid grid, int rotation)
        //{
        //    if (spriteDatas.Count <= 0)
        //    {
        //        return null;
        //    }
        //    return spriteDatas[rotation % spriteDatas.Count].sprite;
        //}

        public virtual TileObject CreateObject(BaseGrid grid, Coord center, int rotation, CustomSaveData saveData = null)
        {
            return new TileObject(grid, this, center, rotation, saveData);
        }

        private bool IsTileValid(BaseGrid grid, Coord coord, CoordType coordType)
        {
            Tile tile = grid.GetTile(coord);
            if (tile == null)
            {
                return false;
            }
            if (!tile.IsUnblocked)
            {
                return false;
            }
            if (coordType == CoordType.Blocking && !tile.IsPassable)
            {
                return false;
            }
            return true;
        }

        public override bool IsValidCoord(BaseGrid grid, Coord center, int rotation = 0)
        {
            foreach (var placementCoord in coords)
            {
                Coord normalized = center;
                normalized.NormalizeAndRotate(placementCoord.coord, rotation);

                if (!IsTileValid(grid, normalized, placementCoord.type))
                {
                    return false;
                }
            }
            return true;
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