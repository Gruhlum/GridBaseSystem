using System;
using System.Collections;
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

        public void Add(Coord coord, GridObject gridObj)
        {
            if (!gridObjects.TryAdd(coord, gridObj))
            {
                Debug.LogError($"Coord already occupied: {coord} Old: {gridObjects[coord]} New: {gridObj}");
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
        public void Move(Coord oldCoord, Coord targetCoord, GridObject gridObj)
        {
            Remove(oldCoord, gridObj);
            Add(targetCoord, gridObj);
        }
        public bool HasObject<T>(Coord coord) where T : GridObject
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
        public List<Coord> GetEmptyCoords(List<Coord> coords)
        {
            List<Coord> results = new List<Coord>();

            foreach (var coord in coords)
            {
                if (!gridObjects.ContainsKey(coord))
                {
                    results.Add(coord);
                }
            }

            return results;
        }
        public T Get<T>(Coord coord) where T : GridObject
        {
            if (gridObjects.TryGetValue(coord, out GridObject gridObj))
            {
                if (gridObj is T t)
                {
                    return t;
                }
                Debug.Log($"{gridObj} is not of type {nameof(T)}!");
                return null;
            }
            Debug.Log($"{coord} is empty!");
            return null;
        }
        public List<T> Get<T>(IList<Coord> coords) where T : GridObject
        {
            List<T> results = new List<T>();
            foreach (var coord in coords)
            {
                results.Add(Get<T>(coord));
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
        public List<GridObject> Get(IList<Coord> coords)
        {
            List<GridObject> results = new List<GridObject>();
            foreach (var coord in coords)
            {
                results.Add(Get(coord));
            }
            return results;
        }
        public IEnumerable<GridObject> GetAll()
        {
            HashSet<GridObject> results = new HashSet<GridObject>();
            foreach (var gridObj in gridObjects.Values)
            {
                if (!results.Contains(gridObj))
                {
                    results.Add(gridObj);
                }
            }
            return results;
        }
        public IEnumerable<T> GetAll<T>() where T : GridObject
        {
            HashSet<T> results = new HashSet<T>();
            foreach (var gridObj in gridObjects.Values)
            {
                if (!results.Contains(gridObj) && gridObj is T t)
                {
                    results.Add(t);
                }
            }
            return results;
        }
    }
}