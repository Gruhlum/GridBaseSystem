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
        private Dictionary<Coord, GridObjectBase> gridObjects;

        public GridLayer()
        {
            gridObjects = new Dictionary<Coord, GridObjectBase>();
        }
        public GridLayer(Coord coord, GridObjectBase gridObj) : this()
        {
            Add(coord, gridObj);
        }

        public void Add(Coord coord, GridObjectBase gridObj)
        {
            if (!gridObjects.TryAdd(coord, gridObj))
            {
                Debug.LogError($"Coord already occupied: {coord} ({gridObjects[coord].Name}) {gridObj}");
            }
        }
        public void Remove(Coord coord, GridObjectBase gridObj)
        {
            if (gridObjects.TryGetValue(coord, out GridObjectBase result))
            {
                if (result == gridObj)
                {
                    gridObjects.Remove(coord);
                }
                else Debug.Log($"Wrong owner for {coord}! {gridObj} is trying to remove {result}");
            }
            else Debug.Log($"Can't remove Object, {coord} is already empty!");
        }
        public void Move(Coord oldCoord, Coord targetCoord, GridObjectBase gridObj)
        {
            Remove(oldCoord, gridObj);
            Add(targetCoord, gridObj);
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
        public T Get<T>(Coord coord) where T : GridObjectBase
        {
            if (gridObjects.TryGetValue(coord, out GridObjectBase gridObj))
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
        public List<T> Get<T>(IList<Coord> coords) where T : GridObjectBase
        {
            List<T> results = new List<T>();
            foreach (var coord in coords)
            {
                results.Add(Get<T>(coord));
            }
            return results;
        }
        public GridObjectBase Get(Coord coord)
        {
            if (gridObjects.TryGetValue(coord, out GridObjectBase gridObj))
            {
                return gridObj;
            }
            return null;
        }
        public List<GridObjectBase> Get(IList<Coord> coords)
        {
            List<GridObjectBase> results = new List<GridObjectBase>();
            foreach (var coord in coords)
            {
                results.Add(Get(coord));
            }
            return results;
        }
        public IEnumerable<GridObjectBase> GetAll()
        {
            return gridObjects.Values;
        }
        public IEnumerable<T> GetAll<T>() where T : GridObjectBase
        {
            List<T> results = new List<T>();
            foreach (var gridObj in gridObjects.Values)
            {
                if (gridObj is T t)
                {
                    results.Add(t);
                }
            }
            return results;
        }
    }
}