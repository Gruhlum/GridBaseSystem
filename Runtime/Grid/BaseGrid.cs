using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class BaseGrid : MonoBehaviour
    {
        public Coord[,] Coordinates
        {
            get
            {
                return coordinates;
            }
        }
        protected Coord[,] coordinates;
        protected TileObject[,] tileObjects;

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


        public event Action<Coord> OnTileAdded;
        public event Action<Coord> OnTileRemoved;
        public event Action OnGridGenerated;

        public event Action<TileObject> OnTileObjectAdded;
        public event Action<TileObject> OnTileObjectRemoved;
        public event Action<TileObject> OnTileObjectMoved;

        
        public float TileWidth
        {
            get
            {
                return tileWidth;
            }
            set
            {
                tileWidth = value;
            }
        }
        [Header("Tile Data")][SerializeField] private float tileWidth = 1;

        public float TileHeight
        {
            get
            {
                return tileHeight;
            }
            set
            {
                tileHeight = value;
            }
        }
        [SerializeField] private float tileHeight = 1;

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

        public float TotalVerticalSpacing
        {
            get
            {
                return VerticalSpacing + TileWidth;
            }
        }
        public float TotalHorizontalSpacing
        {
            get
            {
                return HorizontalSpacing + TileHeight;
            }
        }


        private void Awake()
        {
            //GenerateGrid(15, 15);
        }

        //public virtual void GenerateGrid(List<Coord> coords)
        //{

        //}
        public virtual void GenerateGrid(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            coordinates = new Coord[width, height];
            tileObjects = new TileObject[width, height];
            center.Set(width / 2, height / 2);

            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    coordinates[w, h].x = w;
                    coordinates[w, h].y = h;
                    coordinates[w, h].isValid = true;
                }
            }
            transform.position = new Vector2(-(width - 1) / 2f * TotalVerticalSpacing, -(height - 1) / 2f * TotalHorizontalSpacing);
            OnGridGenerated?.Invoke();
        }

        public void AddTile(Coord coord)
        {
            AddTile(coord.x, coord.y);
        }
        public void AddTile(int x, int y)
        {
            if (Width <= x || Height <= y)
            {
                ResizeCoordinatesArray(x, y);
            }
            Coordinates[x, y].isValid = true;
            OnTileAdded?.Invoke(Coordinates[x, y]);
        }
        public void RemoveTile(Coord coord)
        {
            RemoveTile(coord.x, coord.y);
        }
        public void RemoveTile(int x, int y)
        {
            if (Width <= x || Height <= y)
            {
                Debug.Log("Trying to remove tile that is out of bounds");
                return;
            }
            Coordinates[x, y].isValid = false;
            OnTileRemoved?.Invoke(Coordinates[x, y]);
        }

        private void ResizeCoordinatesArray(int width, int height)
        {
            int minWidth = Mathf.Min(width, Width);
            int minHeight = Mathf.Min(height, Height);

            Coord[,] results = new Coord[minWidth, minHeight];

            for (int x = 0; x < minWidth; x++)
            {
                for (int y = 0; y < minHeight; y++)
                {
                    if (Width > x && Height > y)
                    {
                        results[x, y] = Coordinates[x, y];
                    }
                    else results[x, y].isValid = false;
                }
            }
            coordinates = results;
        }
        public abstract Vector3 CoordToWorldPoint(Coord coord);
        public List<Vector3> CoordsToWorldPoint(List<Coord> coords)
        {
            List<Vector3> results = new List<Vector3>(coords.Count);
            foreach (var coord in coords)
            {
                results.Add(CoordToWorldPoint(coord));
            }
            return results;
        }
        public virtual Coord MousePositionToCoord()
        {
            return WorldPositionToCoord(Camera.main.GetMousePosition());
        }
        public abstract Coord WorldPositionToCoord(Vector3 position);

        public TileObject GetTileObject(Coord coord)
        {
            if (!DoesTileExist(coord))
            {
                return null;
            }
            return tileObjects[coord.x, coord.y];
        }
        //public TileObject GetTileObject<T>(Coord coord) where T : TileObject
        //{
        //    return tileObjects[coord.x, coord.y] as T;
        //}
        public List<TileObject> GetTileObjects(List<Coord> coords)
        {
            List<TileObject> results = new List<TileObject>();

            foreach (var coord in coords)
            {
                var tileObj = GetTileObject(coord);
                if (tileObj != null)
                {
                    results.Add(tileObj);
                }
            }
            return results;
        }
        private void AssignCoord(TileObject obj)
        {
            List<Coord> coords = obj.GetNormalizedCoords();
            foreach (var coord in coords)
            {
                if (!IsTileEmpty(coord))
                {
                    Debug.LogError("position is already taken or invalid: " + coord.ToString());
                    return;
                }
                tileObjects[coord.x, coord.y] = obj;
            }
        }
        public void AddTileObject(TileObject obj)
        {
            AssignCoord(obj);
            OnTileObjectAdded?.Invoke(obj);
        }
        private void ClearObjectCoord(TileObject obj)
        {
            List<Coord> coords = obj.GetNormalizedCoords();
            foreach (var coord in coords)
            {
                tileObjects[coord.x, coord.y] = null;
            }
        }
        public void RemoveTileObject(TileObject obj)
        {
            ClearObjectCoord(obj);
            OnTileObjectRemoved(obj);
        }
        public void MoveTileObject(TileObject obj, Coord oldCoord)
        {
            var coords = obj.GetNormalizedCoords(oldCoord);
            foreach (var coord in coords)
            {
                if (!DoesTileExist(coord))
                {
                    Debug.LogError("position is invalid: " + coord.ToString());
                    return;
                }
                tileObjects[coord.x, coord.y] = null;
            }
            AssignCoord(obj);
            OnTileObjectMoved?.Invoke(obj);
        }

        public List<Coord> GetValidCoords(List<Coord> coords)
        {
            for (int i = coords.Count - 1; i >= 0; i--)
            {
                if (!DoesTileExist(coords[i]))
                {
                    coords.RemoveAt(i);
                }
            }
            return coords;
        }
        public List<Coord> GetEmptyCoords(List<Coord> coords)
        {
            for (int i = coords.Count - 1; i >= 0; i--)
            {
                if (!IsTileEmpty(coords[i]))
                {
                    coords.RemoveAt(i);
                }
            }
            return coords;
        }
        public bool IsTileEmpty(Coord coord)
        {
            if (!DoesTileExist(coord))
            {
                return false;
            }
            return tileObjects[coord.x, coord.y] == null;
        }
        public bool IsTilePassable(Coord coord)
        {
            if (!DoesTileExist(coord))
            {
                return false;
            }
            if (IsTileEmpty(coord))
            {
                return true;
            }
            return !GetTileObject(coord).Data.IsWall;
        }
        public bool DoesTileExist(Coord coord)
        {
            if (coordinates == null)
            {
                return false;
            }

            if (coord.x >= 0 && coord.y >= 0 && Width > coord.x && Height > coord.y)
            {
                //Debug.Log(lengthX + " - " + normalized.x + " - " + lengthY + " - " + normalized.y);
                return coordinates[coord.x, coord.y].isValid;
            }
            return false;
        }

        public List<List<Coord>> GetAllConnectedCoords()
        {
            List<List<Coord>> results = new List<List<Coord>>();
            int totalResults = 0;
            DateTime startTime = DateTime.Now;
            foreach (var coord in coordinates)
            {
                if (results.Any(x => x.Contains(coord)))
                {
                    continue;
                }
                var connectedCoords = GetConnectedCoords(coord);
                if (connectedCoords.Count == 0)
                {
                    //Debug.Log("is empty! " + coord.ToString());
                    continue;
                }
                totalResults += connectedCoords.Count;
                results.Add(connectedCoords);
                if (totalResults >= coordinates.Length)
                {
                    Debug.Log("Finished in: " + (DateTime.Now - startTime) + " ms");
                    return results;
                }
            }
            Debug.Log("Finished in: " + (DateTime.Now - startTime) + " ms");
            return results;

        }

        public List<Coord> GetConnectedCoords(Coord start)
        {
            if (!IsTilePassable(start))
            {
                return new List<Coord>();
            }
            return GetCoordsInLine(start, true, new List<Coord>());
        }

        private List<Coord> GetCoordsInLine(Coord start, bool diagonal, List<Coord> results)
        {
            List<Coord> newResults = new List<Coord>();
            Coord currentCoord = start;
            //if (diagonal)
            //{
            //    currentCoord.x += 1;
            //}
            //else currentCoord.y += 1;

            while (IsTilePassable(currentCoord) && !results.Contains(currentCoord))
            {
                //Debug.Log(currentCoord.ToString());
                newResults.Add(currentCoord);
                if (diagonal)
                {
                    currentCoord.x += 1;
                }
                else currentCoord.y += 1;
            }

            currentCoord = start;
            if (diagonal)
            {
                currentCoord.x -= 1;
            }
            else currentCoord.y -= 1;

            while (IsTilePassable(currentCoord) && !results.Contains(currentCoord))
            {
                newResults.Add(currentCoord);
                if (diagonal)
                {
                    currentCoord.x -= 1;
                }
                else currentCoord.y -= 1;
            }
            results.AddRange(newResults);

            foreach (var newResult in newResults)
            {
                GetCoordsInLine(newResult, !diagonal, results);
            }
            return results;
        }

        //public Coord[][] GetPathMap(Coord start, int maximumDistance = 200)
        //{
        //    List<Coord> checkCoords = new List<Coord>() { start };
        //    Coord[][] pathMap = new Coord[maximumDistance][];

        //    var neighbours = GetNeighbourCoords(start);
        //    for (int j = 0; j < maximumDistance; j++)
        //    {
        //        for (int i = 0; i < neighbours.Count; i++)
        //        {
        //            Coord neighbour = neighbours[i];
        //            if (checkCoords.Contains(neighbour))
        //            {
        //                neighbours.RemoveAt(i);
        //                continue;
        //            }
        //            if (!DoesTileExist(neighbour))
        //            {
        //                neighbours.RemoveAt(i);
        //                continue;
        //            }
        //            TileObject neighbourObj = GetTileObject(neighbour);
        //            if (neighbourObj != null && neighbourObj.Data.IsWall)
        //            {
        //                neighbours.RemoveAt(i);
        //                continue;
        //            }
        //            checkCoords.Add(neighbour);
        //        }
        //        pathMap[j] = neighbours.ToArray();
        //    }         
        //    return pathMap;
        //}
        public abstract List<Coord> GetArea(Coord center, int radius);
        public abstract List<Coord> GetRing(Coord center, int radius);
        public abstract List<Coord> GetNeighbourCoords(Coord center);        
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
        public virtual List<TileObject> GetNeighbourObjects(Coord center)
        {
            var coords = GetNeighbourCoords(center);
            List<TileObject> results = new List<TileObject>();
            foreach (var coord in coords)
            {
                TileObject tileObj = GetTileObject(coord);
                if (tileObj != null)
                {
                    results.Add(tileObj);
                }
            }
            return results;
        }
        public virtual List<TileObject> GetNeighbourObjects(List<Coord> coords)
        {
            var resultCoords = GetNeighbourCoords(coords);
            List<TileObject> results = new List<TileObject>();
            foreach (var coord in resultCoords)
            {
                TileObject tileObj = GetTileObject(coord);
                if (tileObj != null)
                {
                    results.Add(tileObj);
                }
            }
            return results;
        }
        public virtual List<T> GetNeighbourObjects<T>(List<Coord> coords) where T : TileObject
        {
            var resultCoords = GetNeighbourCoords(coords);
            List<T> results = new List<T>();
            foreach (var coord in resultCoords)
            {
                TileObject tileObj = GetTileObject(coord);
                if (tileObj != null && tileObj is T t)
                {
                    results.Add(t);
                }
            }
            return results;
        }
    }
}