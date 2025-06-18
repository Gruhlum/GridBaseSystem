using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectData<T, D, V, S> : GridObjectDataBase
        where T : GridObject<T, D, V, S> where D : GridObjectData<T, D, V, S> where V : GridObjectVisual<T, D, V, S> where S : GridObjectSaveData<T, D, V, S>
    {
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

        public virtual GridObjectVisualBase CreateVisual(T t, BaseGrid grid)
        {
            if (spawner.Prefab == null)
            {
                spawner.Prefab = VisualPrefab;
            }
            V visual = spawner.Spawn();
            SetupVisual(visual, t, grid);
            return visual;
        }
        public sealed override GridObjectVisualBase CreateVisual(GridObjectBase obj, BaseGrid grid)
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