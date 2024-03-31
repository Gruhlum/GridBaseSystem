using System;
using System.Collections;
using System.Collections.Generic;
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
        protected BaseTileObject[,] tileObjects;

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

        public event Action<Coord> OnTileAdded;
        public event Action<Coord> OnTileRemoved;
        public event Action OnGridGenerated;

        public event Action<BaseTileObject> OnTileObjectAdded;
        public event Action<BaseTileObject> OnTileObjectRemoved;
        public event Action<BaseTileObject> OnTileObjectMoved;


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

        public virtual void GenerateGrid(List<Coord> coords)
        {

        }
        public virtual void GenerateGrid(int width, int height)
        {
            coordinates = new Coord[width, height];
            tileObjects = new BaseTileObject[width, height];
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
            if (Coordinates.GetLength(0) <= x || Coordinates.GetLength(1) <= y)
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
            if (Coordinates.GetLength(0) <= x || Coordinates.GetLength(1) <= y)
            {
                Debug.Log("Trying to remove tile that is out of bounds");
                return;
            }
            Coordinates[x, y].isValid = false;
            OnTileRemoved?.Invoke(Coordinates[x, y]);
        }

        private void ResizeCoordinatesArray(int width, int height)
        {
            int minWidth = Mathf.Min(width, Coordinates.GetLength(0));
            int minHeight = Mathf.Min(height, Coordinates.GetLength(1));

            Coord[,] results = new Coord[minWidth, minHeight];

            for (int x = 0; x < minWidth; x++)
            {
                for (int y = 0; y < minHeight; y++)
                {
                    if (coordinates.GetLength(0) > x && coordinates.GetLength(1) > y)
                    {
                        results[x, y] = Coordinates[x, y];
                    }
                    else results[x, y].isValid = false;
                }
            }
            coordinates = results;
        }
        public abstract Vector3 CoordToWorldPoint(Coord coord);

        public virtual Coord MousePositionToCoord()
        {
            return WorldPositionToCoord(Camera.main.GetMousePosition());
        }
        public abstract Coord WorldPositionToCoord(Vector3 position);

        public BaseTileObject GetTileObject(Coord coord)
        {
            return tileObjects[coord.x, coord.y];
        }
        public BaseTileObject GetTileObject<T>(Coord coord) where T : BaseTileObject
        {
            return tileObjects[coord.x, coord.y] as T;
        }
        private void AssignCoord(BaseTileObject obj)
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
        public void AddTileObject(BaseTileObject obj)
        {
            AssignCoord(obj);
            OnTileObjectAdded?.Invoke(obj);
        }
        private void ClearObjectCoord(BaseTileObject obj)
        {
            List<Coord> coords = obj.GetNormalizedCoords();
            foreach (var coord in coords)
            {
                tileObjects[coord.x, coord.y] = null;
            }          
        }
        public void RemoveTileObject(BaseTileObject obj)
        {
            ClearObjectCoord(obj);
            OnTileObjectRemoved(obj);
        }
        public void MoveTileObject(BaseTileObject obj, Coord oldCoord)
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
        public bool DoesTileExist(Coord coord)
        {
            int lengthX = coordinates.GetLength(0);
            int lengthY = coordinates.GetLength(1);

            if (coord.x >= 0 && coord.y >= 0 && lengthX > coord.x && lengthY > coord.y)
            {
                //Debug.Log(lengthX + " - " + normalized.x + " - " + lengthY + " - " + normalized.y);
                return coordinates[coord.x, coord.y].isValid;
            }
            return false;
        }

        public abstract List<Coord> GetArea(Coord center, int radius);
        public abstract List<Coord> GetRing(Coord center, int radius);
        public virtual List<Coord> GetNeighbours(Coord center)
        {
            return GetRing(center, 1);
        }
    }
}