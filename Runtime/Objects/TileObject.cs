using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class TileObject
    {
        public BaseGrid Grid
        {
            get
            {
                return grid;
            }
        }
        protected BaseGrid grid;
        public BaseTileObjectData Data
        {
            get
            {
                return data;
            }
        }
        private BaseTileObjectData data;

        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                if (sprite == value)
                {
                    return;
                }
                sprite = value;
                OnSpriteChanged?.Invoke(this, sprite);
            }
        }
        private Sprite sprite;

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnColorChanged?.Invoke(this, color);
            }
        }
        private Color color = Color.white;


        public Coord Center
        {
            get
            {
                return center;
            }
        }
        private Coord center;

        public event Action<TileObject, Sprite> OnSpriteChanged;
        public event Action<TileObject, Color> OnColorChanged;
        public event Action<TileObject> OnRemoved;
        public event Action<TileObject> OnMoved;

        public TileObject(Coord center, BaseGrid grid, BaseTileObjectData data)
        {
            this.grid = grid;
            this.data = data;
            this.center = center;
            sprite = data.GetSprite();
        }

        public virtual void Remove()
        {
            grid.RemoveTileObject(this);
            OnRemoved?.Invoke(this);
        }
        public virtual void Move(Coord target)
        {
            Coord oldCenter = center;
            center = target;
            grid.MoveTileObject(this, oldCenter);
            OnMoved?.Invoke(this);
        }

        public List<Coord> GetNormalizedCoords()
        {
            return GetNormalizedCoords(center);
        }

        public List<Coord> GetNormalizedCoords(Coord center)
        {
            var coords = data.GetCoords();
            for (int i = 0; i < coords.Count; i++)
            {
                coords[i] += center;
            }
            return coords;
        }

        public List<Coord> GetAllNeighbourCoords(List<Coord> coords)
        {
            List<Coord> neighbours = new List<Coord>();

            foreach (var coord in coords)
            {
                var results = grid.GetNeighbourCoords(coord);
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

        public Vector3 GetWorldPosition()
        {
            return grid.CoordToWorldPoint(Center);
        }
    }
}