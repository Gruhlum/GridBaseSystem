using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class BaseGrid : MonoBehaviour
    {
        public Tile[,] Coordinates
        {
            get
            {
                return coordinates;
            }
        }
        protected Tile[,] coordinates = new Tile[1, 1];

        protected TileObject[,] tileObjects = new TileObject[1,1];

        public bool AllowResize
        {
            get
            {
                return allowResize;
            }
            private set
            {
                allowResize = value;
            }
        }
        [SerializeField] private bool allowResize;

        public int MaximumWidth
        {
            get
            {
                return maximumWidth;
            }
            private set
            {
                maximumWidth = value;
            }
        }
        [SerializeField] private int maximumWidth;

        public int MaximumHeight
        {
            get
            {
                return maximumHeight;
            }
            private set
            {
                maximumHeight = value;
            }
        }
        [SerializeField] private int maximumHeight;

        public int StartWidth
        {
            get
            {
                return startWidth;
            }
            private set
            {
                startWidth = value;
            }
        }
        [SerializeField] private int startWidth;

        public int StartHeight
        {
            get
            {
                return startHeight;
            }
            private set
            {
                startHeight = value;
            }
        }
        [SerializeField] private int startHeight;


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


        public event Action<Tile> OnTileAdded;
        public event Action<Tile> OnTileRemoved;
        public event Action OnGridGenerated;

        public event Action<TileObject> OnTileObjectAdded;
        public event Action<TileObject> OnTileObjectRemoved;
        public event Action<TileObject> OnTileObjectMoved;

        //public event Action<Tile> OnTileRemoved;

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

        //public virtual void GenerateGrid(int width, int height)
        //{
        //    SetupBounds(width, height);

        //    for (int w = 0; w < width; w++)
        //    {
        //        for (int h = 0; h < height; h++)
        //        {
        //            SetupCoordinate(w, h);
        //        }
        //    }
        //    OnGridGenerated?.Invoke();
        //}


        void Awake()
        {
            if (StartHeight > 0 && StartWidth > 0)
            {
                SetupBounds(StartWidth, StartHeight);
            }
        }

        public virtual void GenerateGrid(List<Tile> tiles)
        {
            SetupBounds(tiles.Max(c => c.X + 1), tiles.Max(c => c.Y) + 1);
            foreach (var tile in tiles)
            {
                coordinates[tile.X, tile.Y] = tile;
            }
            OnGridGenerated?.Invoke();
        }

        private void SetupBounds(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            coordinates = new Tile[width, height];
            tileObjects = new TileObject[width, height];
            center.Set(width / 2, height / 2);
            transform.position = new Vector2(-(width - 1) / 2f * TotalVerticalSpacing, -(height - 1) / 2f * TotalHorizontalSpacing);
        }

        public void AddTile(Coord coord, TileData data)
        {
            AddTile(new Tile(coord, this, data));
        }   
        public void AddTile(Tile tile)
        {
            if (Width <= tile.X || Height <= tile.Y)
            {
                ResizeCoordinatesArray(tile.X + 1, tile.Y + 1);
            }
            Coordinates[tile.X, tile.Y] = tile;
            UpdateTileNeighbours(tile);
            OnTileAdded?.Invoke(tile);
        }
        private void UpdateTileNeighbours(Tile t)
        {
            List<Tile> neighbours = GetNeighbourTiles(t.Center);
            foreach (var neighbour in neighbours)
            {
                neighbour.UpdateSprite();
            }
        }
        public void RemoveGridObject(Coord coord)
        {
            if (!DoesTileExist(coord))
            {
                return;
            }
            if (tileObjects[coord.x, coord.y] != null)
            {
                RemoveTileObject(tileObjects[coord.x, coord.y]);
            }
            else if (Coordinates[coord.x, coord.y] != null)
            {
                RemoveTile(coord);
            }
        }
        public void RemoveTile(Tile tile)
        {
            RemoveTile(tile.X, tile.Y);
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
            Tile tile = Coordinates[x, y];
            Coordinates[x, y] = null;
            UpdateTileNeighbours(tile);
            OnTileRemoved?.Invoke(tile);
        }
        private void ResizeCoordinatesArray(int width, int height)
        {
            int minWidth = Mathf.Max(width, Width);
            int minHeight = Mathf.Max(height, Height);

            Tile[,] results = new Tile[minWidth, minHeight];

            for (int x = 0; x < minWidth; x++)
            {
                for (int y = 0; y < minHeight; y++)
                {
                    if (Width > x && Height > y)
                    {
                        results[x, y] = Coordinates[x, y];
                    }
                }
            }
            coordinates = results;

            TileObject[,] tileObjs = new TileObject[minWidth, minHeight];

            for (int x = 0; x < minWidth; x++)
            {
                for (int y = 0; y < minHeight; y++)
                {
                    if (Width > x && Height > y)
                    {
                        tileObjs[x, y] = tileObjects[x, y];
                    }
                }
            }
            tileObjects = tileObjs;


            Width = minWidth;
            Height = minHeight;
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
     
        public void AddGridObject(GridObject obj)
        {
            if (obj is Tile tile)
            {
                AddTile(tile);
            }
            else if (obj is TileObject tileObj)
            {
                AddTileObject(tileObj);
            }
        }
        public void AddTileObject(TileObject obj)
        {
            AddTileObjectCoords(obj, obj.GetNormalizedCoords());
            OnTileObjectAdded?.Invoke(obj);
        }
        public void AddTileObjectCoords(TileObject obj, List<Coord> coords)
        {
            foreach (var coord in coords)
            {
                tileObjects[coord.x, coord.y] = obj;
            }
        }
        public void RemoveTileObject(TileObject obj)
        {
            List<Coord> coords = obj.GetNormalizedCoords();
            RemoveTileObjectCoords(coords);
            OnTileObjectRemoved?.Invoke(obj);
        }
        private void RemoveTileObjectCoords(List<Coord> coords)
        {
            foreach (var coord in coords)
            {
                tileObjects[coord.x, coord.y] = null;
            }
        }
        public void MoveTileObject(TileObject obj, Coord oldCenter)
        {
            var oldCoords = obj.GetNormalizedCoords(oldCenter);
            RemoveTileObjectCoords(oldCoords);
            AddTileObjectCoords(obj, obj.GetNormalizedCoords());
            OnTileObjectMoved?.Invoke(obj);
        }

        public List<Tile> GetValidTiles(List<Coord> coords)
        {
            for (int i = coords.Count - 1; i >= 0; i--)
            {
                if (!DoesTileExist(coords[i]))
                {
                    coords.RemoveAt(i);
                }
            }
            return GetTiles(coords);
        }
        public List<Tile> GetEmptyTiles(List<Coord> coords)
        {
            for (int i = coords.Count - 1; i >= 0; i--)
            {
                if (!IsTileEmpty(coords[i]))
                {
                    coords.RemoveAt(i);
                }
            }
            return GetTiles(coords);
        }
        public Tile GetTile(Coord coord)
        {
            if (IsCoordOutOfBounds(coord))
            {
                return null;
            }
            return Coordinates[coord.x, coord.y];
        }
        private bool IsCoordOutOfBounds(Coord coord)
        {
            if (coord.x < 0 || coord.y < 0 || coord.x > Width || coord.y > Height)
            {
                return true;
            }
            return false;
        }
        public List<Tile> GetTiles(List<Coord> coords)
        {
            List<Tile> results = new List<Tile>();
            foreach (var coord in coords)
            {
                if (Coordinates[coord.x, coord.y] != null)
                {
                    results.Add(Coordinates[coord.x, coord.y]);
                }
            }
            return results;
        }
        public List<Tile> GetAllTiles()
        {
            var results = new List<Tile>();
            for (int x = 0; x < coordinates.GetLength(0); x++)
            {
                for (int y = 0; y < coordinates.GetLength(1); y++)
                {
                    if (coordinates[x, y] != null)
                    {
                        results.Add(coordinates[x, y]);
                    }
                }
            }
            return results;
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
                return coordinates[coord.x, coord.y] != null;
            }
            return false;
        }

        //public List<List<Coord>> GetAllConnectedCoords()
        //{
        //    List<List<Coord>> results = new List<List<Coord>>();
        //    int totalResults = 0;
        //    DateTime startTime = DateTime.Now;
        //    foreach (var coord in coordinates)
        //    {
        //        if (results.Any(x => x.Contains(coord)))
        //        {
        //            continue;
        //        }
        //        var connectedCoords = GetConnectedCoords(coord);
        //        if (connectedCoords.Count == 0)
        //        {
        //            //Debug.Log("is empty! " + coord.ToString());
        //            continue;
        //        }
        //        totalResults += connectedCoords.Count;
        //        results.Add(connectedCoords);
        //        if (totalResults >= coordinates.Length)
        //        {
        //            Debug.Log("Finished in: " + (DateTime.Now - startTime) + " ms");
        //            return results;
        //        }
        //    }
        //    Debug.Log("Finished in: " + (DateTime.Now - startTime) + " ms");
        //    return results;

        //}

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
        public List<Tile> GetNeighbourTiles(Coord center)
        {
            List<Coord> coords = GetNeighbourCoords(center);
            List<Tile> results = new List<Tile>();
            foreach (var coord in coords)
            {
                if (DoesTileExist(coord))
                {
                    results.Add(Coordinates[coord.x, coord.y]);
                }
            }
            return results;
        }
        public List<T> GetNeighbourTiles<T>(Coord center) where T : Tile
        {
            List<Coord> coords = GetNeighbourCoords(center);
            List<T> results = new List<T>();
            foreach (var coord in coords)
            {
                if (DoesTileExist(coord))
                {
                    if (Coordinates[coord.x, coord.y] is T t)
                    {
                        results.Add(t);
                    }                  
                }
            }
            return results;
        }
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