using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    /// <summary>
    /// Does everything required to manage a grid. From adding to removing tiles to calculating neighbouring positions.
    /// </summary>
    public abstract class BaseGrid : MonoBehaviour
    {
        public GridEventSystem EventSystem
        {
            get
            {
                return eventSystem;
            }
            private set
            {
                eventSystem = value;
            }
        }
        [SerializeField] private GridEventSystem eventSystem;

        public abstract float TileWidth
        {
            get;
        }
        public abstract float TileHeight
        {
            get;
        }

        public float HorizontalSpacing
        {
            get
            {
                return horizontalSpacing;
            }
            set
            {
                horizontalSpacing = value;
            }
        }
        [SerializeField] private float horizontalSpacing;
        public float VerticalSpacing
        {
            get
            {
                return verticalSpacing;
            }
            set
            {
                verticalSpacing = value;
            }
        }
        [SerializeField] private float verticalSpacing;

        public abstract float TotalHorizontalSpacing
        {
            get;
        }
        public abstract float TotalVerticalSpacing
        {
            get;
        }

        public Coord Center
        {
            get
            {
                return center;
            }
            private set
            {
                center = value;
            }
        }
        private Coord center;

        public int Width
        {
            get
            {
                return width;
            }
            private set
            {
                width = value;
            }
        }
        private int width;

        public int Height
        {
            get
            {
                return height;
            }
            private set
            {
                height = value;
            }
        }
        private int height;


        protected readonly Dictionary<int, GridLayer> allObjects = new Dictionary<int, GridLayer>();

        public event Action<GridObjectBase> OnGridObjectAdded;
        public event Action<GridObjectBase> OnGridObjectRemoved;
        public event Action<GridObjectBase> OnGridObjectMoved;

        public event Action OnGridGenerated;

        public abstract int MaximumRotation
        {
            get;
        }

        protected virtual void OnDestroy()
        {
        }


        public void AddGridObject(IEnumerable<CoordData> coordDatas, GridObjectBase gridObj)
        {
            foreach (var data in coordDatas)
            {
                AddGridObject(data.layer, data.coord, gridObj);
            }
        }
        public void AddGridObject(CoordData coordData, GridObjectBase gridObj)
        {
            AddGridObject(coordData.layer, coordData.coord, gridObj);
        }
        public void AddGridObject(int layerIndex, Coord coord, GridObjectBase gridObj)
        {
            if (allObjects.TryGetValue(layerIndex, out GridLayer layer))
            {
                layer.Add(coord, gridObj);
            }
            else
            {
                allObjects.Add(layerIndex, new GridLayer(coord, gridObj));
            }
        }

        public void RemoveGridObject(IEnumerable<CoordData> coordDatas, GridObjectBase gridObj)
        {
            foreach (var data in coordDatas)
            {
                RemoveGridObject(data, gridObj);
            }
        }
        public void RemoveGridObject(CoordData coordData, GridObjectBase gridObj)
        {
            RemoveGridObject(coordData.layer, coordData.coord, gridObj);
        }
        public void RemoveGridObject(int layerIndex, Coord coord, GridObjectBase gridObj)
        {
            if (allObjects.TryGetValue(layerIndex, out GridLayer layer))
            {
                layer.Remove(coord, gridObj);
            }
        }

        public T GetGridObject<T>(int layerIndex, Coord coord) where T : GridObjectBase
        {
            if (allObjects.TryGetValue(layerIndex, out GridLayer layer))
            {
                return layer.Get<T>(coord);
            }
            Debug.Log($"Layer {layerIndex} does not exist!");
            return null;
        }
        public GridObjectBase GetGridObject(int layerIndex, Coord coord)
        {
            if (allObjects.TryGetValue(layerIndex, out GridLayer layer))
            {
                return layer.Get(coord);
            }
            Debug.Log($"Layer {layerIndex} does not exist!");
            return null;
        }

        public List<T> GetGridObjects<T>(int layerIndex, List<Coord> coords) where T : GridObjectBase
        {
            if (allObjects.TryGetValue(layerIndex, out GridLayer layer))
            {
                return layer.Get<T>(coords);
            }
            return null;
        }   
        public List<GridObjectBase> GetGridObjects(int layerIndex, List<Coord> coords)
        {
            List<GridObjectBase> results = new List<GridObjectBase>();
            if (allObjects.TryGetValue(layerIndex, out GridLayer layer))
            {
                return layer.Get(coords);
            }
            return null;
        }

        public IEnumerable<T> GetAllGridObjects<T>(int layerIndex) where T : GridObjectBase
        {
            if (allObjects.TryGetValue(layerIndex, out GridLayer layer))
            {
                return layer.GetAll<T>();
            }
            return null;
        }
        public IEnumerable<GridObjectBase> GetAllGridObjects(int layerIndex)
        {
            List<GridObjectBase> allGridObjects = new List<GridObjectBase>();
            if (allObjects.TryGetValue(layerIndex, out GridLayer layer))
            {
                return layer.GetAll();
            }
            else return null;
        }
        public IEnumerable<GridObjectBase> GetAllGridObjects()
        {
            List<GridObjectBase> allGridObjects = new List<GridObjectBase>();
            foreach (var item in allObjects.Values)
            {
                allGridObjects.AddRange(item.GetAll());
            }
            return allGridObjects;
        }

        public void MoveGridObject(int layerIndex, Coord oldCoord, Coord targetCoord, GridObjectBase gridObj)
        {
            if (allObjects.TryGetValue(layerIndex, out GridLayer layer))
            {
                layer.Move(oldCoord, targetCoord, gridObj);
            }
        }

        public bool IsEmpty(int layerIndex, Coord coord)
        {
            if (allObjects.TryGetValue(layerIndex, out GridLayer layer))
            {
                return layer.IsEmpty(coord);
            }
            else return true;
        }

        /// <summary>
        /// Converts a Coord position to WorldPosition.
        /// </summary>
        /// <returns>Vector3 WorldPosition</returns>
        public abstract Vector3 CoordToWorldPosition(Coord coord);

        /// <summary>
        /// Converts a list of Coord positions to WorldPositions.
        /// </summary>
        /// <returns>List of Vector3 WorldPositions</returns>
        public List<Vector3> CoordsToWorldPositions(List<Coord> coords)
        {
            List<Vector3> results = new List<Vector3>(coords.Count);
            foreach (var coord in coords)
            {
                results.Add(CoordToWorldPosition(coord));
            }
            return results;
        }
        /// <summary>
        /// Converts the current mouse position into a Coord position.
        /// </summary>
        /// <returns>Mouse position converted to Coord</returns>
        public Coord MousePositionToCoord()
        {
            return WorldPositionToCoord(Camera.main.GetMousePosition());
        }
        public abstract Coord WorldPositionToCoord(Vector3 position);

        public Coord GetDirectionCoord(Coord coord1, Coord coord2)
        {
            int direction = GetDirection(coord1, coord2);
            return GetDirectionCoord(direction);
        }
        public abstract Coord GetDirectionCoord(int direction);
        public abstract int GetDirection(Coord center, Coord coord);
        public float DirectionToDegrees(int direction)
        {
            return direction * (-360f / MaximumRotation);
        }
        public abstract Coord GetDirectionFromInput(Vector2 input);
       

        public abstract List<Coord> GetCoordsInBox(Vector2 start, Vector2 end);
        public List<Coord> GetRotatedCoords(Coord center, List<Coord> coords, int rotation)
        {
            List<Coord> results = new List<Coord>();
            foreach (var coord in coords)
            {
                results.Add(GetRotatedCoord(center, coord, rotation));
            }
            return results;
        }
        public abstract Coord GetRotatedCoord(Coord center, Coord coord, int rotation);
        public abstract List<Coord> GetArea(Coord center, int radius);
        public List<Coord> GetArea(int radius)
        {
            return GetArea(Center, radius);
        }
        public abstract List<Coord> GetRing(Coord center, int radius);
        public List<Coord> GetRing(int radius)
        {
            return GetRing(Center, radius);
        }
        public abstract List<Coord> GetNeighbourCoords(Coord center);
        public abstract List<Coord> GetAdjacents(Coord center);
        public List<Coord> GetNeighbourCoords(List<Coord> coords)
        {
            List<Coord> neighbours = new List<Coord>();

            foreach (var coord in coords)
            {
                var results = GetNeighbourCoords(coord);
                foreach (var result in results)
                {
                    if (!coords.Contains(result) && !neighbours.Contains(result))
                    {
                        neighbours.Add(result);
                    }
                }
            }
            return neighbours;
        }
        public abstract List<Coord> GetCorner(int direction, int radius, int thickness);
        public abstract Coord GetClosestCoordInLine(Coord start, Coord target, int dragDirection);
        public abstract List<Coord> GetLine(Coord coord, Coord mouseCoord);
        public abstract bool IsInLine(Coord coord1, Coord coord2);
        public abstract int GetDistance(Coord coord1, Coord coord2);

        //public virtual List<TileObject> GetNeighbourObjects(Coord center)
        //{
        //    var coords = GetNeighbourCoords(center);
        //    List<TileObject> results = new List<TileObject>();
        //    foreach (var coord in coords)
        //    {
        //        TileObject tileObj = GetTileObject(coord);
        //        if (tileObj != null)
        //        {
        //            results.Add(tileObj);
        //        }
        //    }
        //    return results;
        //}
        //public virtual List<TileObject> GetNeighbourObjects(List<Coord> coords)
        //{
        //    var resultCoords = GetNeighbourCoords(coords);
        //    List<TileObject> results = new List<TileObject>();
        //    foreach (var coord in resultCoords)
        //    {
        //        TileObject tileObj = GetTileObject(coord);
        //        if (tileObj != null)
        //        {
        //            results.Add(tileObj);
        //        }
        //    }
        //    return results;
        //}
        //public virtual List<T> GetNeighbourObjects<T>(List<Coord> coords) where T : TileObject
        //{
        //    var resultCoords = GetNeighbourCoords(coords);
        //    List<T> results = new List<T>();
        //    foreach (var coord in resultCoords)
        //    {
        //        TileObject tileObj = GetTileObject(coord);
        //        if (tileObj != null && tileObj is T t)
        //        {
        //            results.Add(t);
        //        }
        //    }
        //    return results;
        //}
    }
}