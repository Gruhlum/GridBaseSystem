using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class TileObjectData<T, D, V, S> : TileObjectDataBase
        where T : TileObject<T, D, V, S> where D : TileObjectData<T, D, V, S> where V : TileObjectVisual<T, D, V, S> where S : TileObjectSaveData<T, D, V, S>
    {


        [SerializeField] public List<PlacementCoord> coords = new List<PlacementCoord>() { new PlacementCoord() };
        //[SerializeField] protected List<SpriteData> spriteDatas = new List<SpriteData>();

        public V VisualPrefab
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
        [SerializeField] private V visualPrefab;

        private SpawnableSpawner<V> spawner = new SpawnableSpawner<V>();


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
        public abstract T GenerateObject(BaseGrid grid, Coord coord, int rotation);

        public sealed override TileObjectBase GenerateTileObject(BaseGrid grid, Coord coord, int rotation)
        {
            return GenerateObject(grid, coord, rotation);
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

        public virtual TileObjectVisualBase CreateVisual(T t, BaseGrid grid)
        {
            if (spawner.Prefab == null)
            {
                spawner.Prefab = VisualPrefab;
            }
            V visual = spawner.Spawn();
            SetupVisual(visual, t, grid);
            return visual;
        }
        public sealed override GridObjectVisual CreateVisual(GridObject obj, BaseGrid grid)
        {
            return CreateVisual(obj as T, grid);
        }
        protected virtual void SetupVisual(V visual, T tileObj, BaseGrid grid)
        {
            visual.Setup(tileObj, grid);
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