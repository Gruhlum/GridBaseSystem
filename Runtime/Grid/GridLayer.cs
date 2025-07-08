using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class GridLayer
    {
        private Dictionary<Coord, GridObject> gridObjects;

        public GridLayer()
        {
            gridObjects = new Dictionary<Coord, GridObject>();
        }
        public GridLayer(Coord coord, GridObject gridObj) : this()
        {
            Add(coord, gridObj);
        }
        public GridLayer(IEnumerable<Coord> coords, GridObject gridObj) : this()
        {
            Add(coords, gridObj);
        }


        public void Add(Coord coord, GridObject gridObj)
        {
            if (!gridObjects.TryAdd(coord, gridObj))
            {
                Debug.LogError($"Coord already occupied: {coord} Old: {gridObjects[coord]} New: {gridObj}");
            }
        }
        public void Add(IEnumerable<Coord> coords, GridObject gridObj)
        {
            foreach (var coord in coords)
            {
                Add(coord, gridObj);
            }
        }
        public void Remove(Coord coord, GridObject gridObj)
        {
            if (gridObjects.TryGetValue(coord, out GridObject result))
            {
                if (result == gridObj)
                {
                    gridObjects.Remove(coord);
                }
                else Debug.Log($"Wrong owner for {coord}! {gridObj} is trying to remove {result}");
            }
            else Debug.Log($"Can't remove Object, {coord} is already empty!");
        }
        public void Remove(IEnumerable<Coord> coords, GridObject gridObj)
        {
            foreach (var coord in coords)
            {
                Remove(coord, gridObj);
            }
        }
        public void Move(Coord oldCoord, Coord targetCoord, GridObject gridObj)
        {
            Remove(oldCoord, gridObj);
            Add(targetCoord, gridObj);
        }

        public bool HasObject<T>(Coord coord)
        {
            if (gridObjects.TryGetValue(coord, out GridObject gridObj))
            {
                return gridObj is T;
            }
            else return false;
        }
        public bool IsEmpty(Coord coord)
        {
            return !gridObjects.ContainsKey(coord);
        }

        public int Count()
        {
            return gridObjects.Count;
        }
        public List<Coord> GetCoords()
        {
            return gridObjects.Keys.ToList();
        }
        public List<Coord> GetEmptyCoords(IEnumerable<Coord> coords)
        {
            List<Coord> results = new List<Coord>();

            foreach (Coord coord in coords)
            {
                if (!gridObjects.ContainsKey(coord))
                {
                    results.Add(coord);
                }
            }

            return results;
        }
        public T Get<T>(Coord coord)
        {
            if (gridObjects.TryGetValue(coord, out GridObject gridObj))
            {
                if (gridObj is T t)
                {
                    return t;
                }
                //Debug.Log($"Coord: {coord} Obj: {gridObj} is not of type {nameof(T)}!");
                return default;
            }
            //Debug.Log($"{coord} is empty!");
            return default;
        }
        public List<T> Get<T>(IEnumerable<Coord> coords)
        {
            List<T> results = new List<T>();
            foreach (Coord coord in coords)
            {
                T result = Get<T>(coord);
                if (result != null)
                {
                    results.Add(result);
                }
            }
            return results;
        }
        public GridObject Get(Coord coord)
        {
            if (gridObjects.TryGetValue(coord, out GridObject gridObj))
            {
                return gridObj;
            }
            return null;
        }
        public List<GridObject> Get(IEnumerable<Coord> coords)
        {
            List<GridObject> results = new List<GridObject>();
            foreach (Coord coord in coords)
            {
                GridObject result = Get(coord);
                if (result != null)
                {
                    results.Add(result);
                }
            }
            return results;
        }
        public HashSet<GridObject> GetAll()
        {
            HashSet<GridObject> results = new HashSet<GridObject>();
            foreach (GridObject gridObj in gridObjects.Values)
            {
                if (!results.Contains(gridObj))
                {
                    results.Add(gridObj);
                }
            }
            return results;
        }
        public HashSet<T> GetAll<T>()
        {
            HashSet<GridObject> results = new HashSet<GridObject>();
            HashSet<T> ts = new HashSet<T>();
            foreach (GridObject gridObj in gridObjects.Values)
            {
                if (!results.Contains(gridObj) && gridObj is T t)
                {
                    results.Add(gridObj);
                    ts.Add(t);
                }
            }
            return ts;
        }
    }
}