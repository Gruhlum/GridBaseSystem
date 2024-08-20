using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


        protected readonly Dictionary<Coord, Tile> tiles = new Dictionary<Coord, Tile>();

        protected readonly List<TileObject> tileObjects = new List<TileObject>();

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


        void Awake()
        {
            //if (StartHeight > 0 && StartWidth > 0)
            //{
            //    SetupBounds(StartWidth, StartHeight);
            //}
        }

        void OnDestroy()
        {
            RemoveAllTiles();
            RemoveAllTileObjects();
        }

        private void RemoveAllTiles()
        {
            List<Coord> keys = tiles.Keys.ToList();
            for (int i = keys.Count - 1; i >= 0; i--)
            {
                tiles[keys[i]].Remove();
            }
        }
        private void RemoveAllTileObjects()
        {
            foreach (var tile in tiles.Values)
            {
                tile.RemoveAllTileObjects();
            }
            foreach (var tileObject in tileObjects)
            {
                tileObject.Remove();
            }
            tileObjects.Clear();
        }

        public virtual void GenerateGrid(List<Tile> tiles)
        {
            //SetupBounds(tiles.Max(c => c.X + 1), tiles.Max(c => c.Y) + 1);
            foreach (var tile in tiles)
            {
                this.tiles.Add(tile.Center, tile);
            }
            foreach (var tile in tiles)
            {
                tile.UpdateSprite();
            }
            OnGridGenerated?.Invoke();
        }

        //private void SetupBounds(int width, int height)
        //{
        //    this.Width = width;
        //    this.Height = height;
        //    MaximumWidth = Mathf.Max(MaximumWidth, this.Width);
        //    MaximumHeight = Mathf.Max(MaximumHeight, this.Height);
        //    coordinates-c
        //    tileObjects = new TileObject[width, height];
        //    center.Set(width / 2, height / 2);
        //    transform.position = new Vector2(-(width - 1) / 2f * TotalVerticalSpacing, -(height - 1) / 2f * TotalHorizontalSpacing);
        //}

        public void AddTile(Coord coord, TileData data)
        {
            AddTile(new Tile(coord, this, data));
        }
        public void AddTile(Tile tile)
        {
            if (tiles.TryGetValue(tile.Center, out Tile otherTile))
            {
                RemoveTile(otherTile);
            }
            tiles[tile.Center] = tile;
            tile.UpdateSprite();
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
            if (tiles.TryGetValue(coord, out Tile tile))
            {
                if (tile.TryGetTileObject(out TileObject obj) && obj.IsReplaceable)
                {
                    RemoveTileObject(obj);
                }
                // else RemoveTile(coord);
            }
        }
        public void RemoveTile(Tile tile)
        {
            RemoveTile(tile.Center);
        }
        public void RemoveTile(Coord coord)
        {
            Tile tile = tiles[coord];
            tiles.Remove(coord);
            UpdateTileNeighbours(tile);
            OnTileRemoved?.Invoke(tile);
        }

        public List<TileObject> GetAllTileObjects()
        {
            return new List<TileObject>(tileObjects);
        }

        //private void ResizeCoordinatesArray(int width, int height)
        //{
        //    int minWidth = Mathf.Max(width, Width);
        //    int minHeight = Mathf.Max(height, Height);

        //    Tile[,] results = new Tile[minWidth, minHeight];

        //    for (int x = 0; x < minWidth; x++)
        //    {
        //        for (int y = 0; y < minHeight; y++)
        //        {
        //            if (Width > x && Height > y)
        //            {
        //                results[x, y] = coordinates[x, y];
        //            }
        //        }
        //    }
        //    coordinatess = results;

        //    TileObject[,] tileObjs = new TileObject[minWidth, minHeight];

        //    for (int x = 0; x < minWidth; x++)
        //    {
        //        for (int y = 0; y < minHeight; y++)
        //        {
        //            if (Width > x && Height > y)
        //            {
        //                tileObjs[x, y] = tileObjects[x, y];
        //            }
        //        }
        //    }
        //    tileObjects = tileObjs;


        //    Width = minWidth;
        //    Height = minHeight;
        //}

        /// <summary>
        /// Converts a Coord position to WorldPosition.
        /// </summary>
        /// <returns>Vector3 WorldPosition</returns>
        public abstract Vector3 CoordToWorldPoint(Coord coord);

        /// <summary>
        /// Converts a list of Coord positions to WorldPositions.
        /// </summary>
        /// <returns>List of Vector3 WorldPositions</returns>
        public List<Vector3> CoordsToWorldPoint(List<Coord> coords)
        {
            List<Vector3> results = new List<Vector3>(coords.Count);
            foreach (var coord in coords)
            {
                results.Add(CoordToWorldPoint(coord));
            }
            return results;
        }
        /// <summary>
        /// Converts the current mouse position into a Coord position.
        /// </summary>
        /// <returns>Mouse position converted to Coord</returns>
        public virtual Coord MousePositionToCoord()
        {
            return WorldPositionToCoord(Camera.main.GetMousePosition());
        }
        public abstract Coord WorldPositionToCoord(Vector3 position);

        /// <summary>
        /// Looks for a TileObject with a specified Coord.
        /// </summary>
        /// <param name="coord">Coord of the TileObject</param>
        /// <returns>The TileObject if found, otherwise null</returns>
        public List<TileObjectPlacementData> GetTileObject(Coord coord)
        {
            if (tiles.TryGetValue(coord, out Tile tile))
            {
                return tile.GetTileObject();
            }
            else return null;
        }
        public List<TileObjectPlacementData> GetTileObjects(List<Coord> coords)
        {
            List<TileObjectPlacementData> results = new List<TileObjectPlacementData>();
            foreach (var coord in coords)
            {
                List<TileObjectPlacementData> result = GetTileObject(coord);
                if (result != null)
                {
                    results.AddRange(result);
                }
            }
            return results;
        }
        ///// <summary>
        ///// Looks for a TileObject with a specified Coord and specified Type.
        ///// </summary>
        ///// <typeparam name="T">Looks for a TileObject of this Type</typeparam>
        ///// <param name="coord">Coord of the TileObject</param>
        ///// <returns>The TileObject if found, otherwise null</returns>
        //public T GetTileObject<T>(Coord coord) where T : TileObject
        //{
        //    if (!DoesTileExist(coord))
        //    {
        //        return null;
        //    }
        //    if (tiles.TryGetValue(coord, out Tile tile))
        //    {
        //        TileObject tileObj = tile.GetTileObject();
        //        if (tileObj is T t)
        //        {
        //            return t;
        //        }
        //    }
        //    return null;
        //}
        //public List<TileObject> GetTileObjects(List<Coord> coords)
        //{
        //    List<TileObject> results = new List<TileObject>();

        //    foreach (var coord in coords)
        //    {
        //        var tileObj = GetTileObject(coord);
        //        if (tileObj != null)
        //        {
        //            results.Add(tileObj);
        //        }
        //    }
        //    return results;
        //}
        //public List<T> GetTileObjects<T>(List<Coord> coords) where T : TileObject
        //{
        //    List<T> results = new List<T>();

        //    foreach (var coord in coords)
        //    {
        //        var tileObj = GetTileObject<T>(coord);
        //        if (tileObj != null)
        //        {
        //            results.Add(tileObj);
        //        }
        //    }
        //    return results;
        //}
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
            AddSafetyCoords(obj);
            AddNormalCoords(obj);
            OnTileObjectAdded?.Invoke(obj);
        }

        private void AddNormalCoords(TileObject obj)
        {
            List<Coord> coords = obj.GetNormalizedCoords();
            foreach (var coord in coords)
            {
                if (tiles.TryGetValue(coord, out Tile tile))
                {
                    tile.AddTileObject(new TileObjectPlacementData(obj, !obj.Data.IsPassable, false));
                }
                else Debug.Log("Invalid Coord: " + coord);
            }
        }

        private void AddSafetyCoords(TileObject obj)
        {
            List<Coord> safetyCoords = obj.GetNormalizedSafeZones();
            foreach (var coord in safetyCoords)
            {
                if (tiles.TryGetValue(coord, out Tile tile))
                {
                    tile.AddTileObject(new TileObjectPlacementData(obj, false, true));
                }
                else Debug.Log("Invalid Coord: " + coord);
            }
        }
        public void RemoveTileObject(TileObject obj)
        {
            tileObjects.Remove(obj);
            RemoveTileObjectCoords(obj, obj.Center);
            obj.Remove();
            OnTileObjectRemoved?.Invoke(obj);
        }

        private void RemoveTileObjectCoords(TileObject obj, Coord center)
        {
            List<Coord> coords = obj.GetNormalizedCoords(center);
            foreach (var tile in GetTiles(coords))
            {
                tile.RemoveTileObject(obj);
            }
        }

        public void MoveTileObject(TileObject obj, Coord oldCenter)
        {
            RemoveTileObjectCoords(obj, oldCenter);
            AddSafetyCoords(obj);
            AddNormalCoords(obj);
            OnTileObjectMoved?.Invoke(obj);
        }

        public abstract Coord GetDirectionCoord(Coord coord, int direction);
        public abstract int GetDirection(Coord center, Coord coord);
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
        private bool IsCoordOutOfBounds(Coord coord)
        {
            if (coord.x < 0 || coord.y < 0 || coord.x >= Width || coord.y >= Height)
            {
                return true;
            }
            return false;
        }
        public Tile GetTile(Coord coord)
        {
            //if (IsCoordOutOfBounds(coord))
            //{
            //    return null;
            //}
            if (!DoesTileExist(coord))
            {
                return null;
            }
            else return tiles[coord];
        }
        public List<Tile> GetTiles(List<Coord> coords)
        {
            List<Tile> results = new List<Tile>();
            foreach (var coord in coords)
            {
                if (tiles.TryGetValue(coord, out Tile tile))
                {
                    results.Add(tile);
                }
            }
            return results;
        }
        public List<Tile> GetAllTiles()
        {
            return tiles.Values.ToList();
        }
        public List<Tile> GetAllTiles(TileData data)
        {
            var results = new List<Tile>();
            foreach (var tile in tiles.Values)
            {
                if (tile.Data == data)
                {
                    results.Add(tile);
                }
            }
            return results;
        }
        public List<T> GetAllTiles<T>() where T : Tile
        {
            var results = new List<T>();
            foreach (var tile in tiles.Values)
            {
                if (tile is T t)
                {
                    results.Add(t);
                }
            }
            return results;
        }
        public bool IsTileSafetyZone(List<Coord> coords)
        {
            foreach (var coord in coords)
            {
                if (IsTileSafetyZone(coord))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsTileSafetyZone(Coord coord)
        {
            if (tiles.TryGetValue(coord, out Tile tile))
            {
                return tile.IsSaveZone();
            }
            else return false;
        }
        public bool IsTileBlocked(List<Coord> coords)
        {
            foreach (var coord in coords)
            {
                if (IsTileBlocked(coord))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsTileBlocked(Coord coord)
        {
            if (tiles.TryGetValue(coord, out Tile tile))
            {
                return tile.IsBlocked();
            }
            else return false;
        }
        public bool CanPlaceBuilding(Coord coord)
        {
            if (tiles.TryGetValue(coord, out Tile tile))
            {
                return !(tile.IsSaveZone() || tile.IsBlocked());
            }
            else return false;
        }
        public bool CanPlaceBuilding(List<Coord> coords)
        {
            foreach (var coord in coords)
            {
                if (!CanPlaceBuilding(coord))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsTileEmpty(List<Coord> coords)
        {
            foreach (var coord in coords)
            {
                if (!IsTileEmpty(coord))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsTileEmpty(Coord coord)
        {
            if (tiles.TryGetValue(coord, out Tile tile))
            {
                return !tile.IsBlocked();
            }
            return false;
        }
        public bool IsTilePassable(Coord coord)
        {           
            if (tiles.TryGetValue(coord, out Tile tile))
            {              
                return tile.IsPassable;
            }
            return false;
        }
        public bool DoesTileExist(Coord coord)
        {
            return tiles.ContainsKey(coord);
        }
        public bool DoesTileExist(List<Coord> coords)
        {
            foreach (var coord in coords)
            {
                if (!DoesTileExist(coord))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsAllowedCoord(Coord coord)
        {
            if (coord.x < 0 || coord.y < 0)
            {
                return false;
            }
            if (coord.x >= maximumWidth || coord.y >= maximumHeight)
            {
                return false;
            }
            return true;
        }

        public List<List<Coord>> GetAllConnectedCoords()
        {
            List<List<Coord>> results = new List<List<Coord>>();
            int totalResults = 0;
            DateTime startTime = DateTime.Now;
            foreach (var tile in tiles.Values)
            {
                if (tile == null)
                {
                    continue;
                }
                if (results.Any(x => x.Contains(tile.Center)))
                {
                    continue;
                }
                var connectedCoords = GetConnectedCoords(tile.Center);
                if (connectedCoords.Count == 0)
                {
                    //Debug.Log("is empty! " + coord.ToString());
                    continue;
                }
                totalResults += connectedCoords.Count;
                results.Add(connectedCoords);

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
        public abstract List<Coord> GetRing(Coord center, int radius);
        public abstract List<Coord> GetNeighbourCoords(Coord center);
        public abstract List<Coord> GetAdjacents(Coord center);
        public List<Tile> GetAdjacentTiles(Coord center)
        {
            var adjacents = GetAdjacents(center);
            List<Tile> results = new List<Tile>();
            foreach (var adjacent in adjacents)
            {
                if (tiles.TryGetValue(adjacent, out Tile tile))
                {
                    results.Add(tile);
                }
            }
            return results;
        }
        public List<Tile> GetNeighbourTiles(Coord center)
        {
            List<Coord> coords = GetNeighbourCoords(center);
            List<Tile> results = new List<Tile>();
            foreach (var coord in coords)
            {
                if (DoesTileExist(coord))
                {
                    results.Add(tiles[coord]);
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
                    if (tiles[coord] is T t)
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