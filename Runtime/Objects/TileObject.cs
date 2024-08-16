using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class TileObject : GridObject
    {
        public BaseTileObjectData Data
        {
            get
            {
                return data;
            }
        }
        private BaseTileObjectData data;


        public TileObject(Coord center, BaseGrid grid, BaseTileObjectData data) : base(center, grid, data)
        {
            this.data = data;
            Sprite = data.GetSprite();
        }          

        public List<Coord> GetRotatedCoords()
        {
            return GetRotatedCoords(Center);
        }
        public List<Coord> GetRotatedCoords(Coord center)
        {
            List<Coord> normalized = GetNormalizedCoords(center);
            return Grid.GetRotatedCoords(center, normalized, Rotation);
        }
        public List<Coord> GetNormalizedCoords(Coord center)
        {
            return center.GetNormalizedCoords(data.GetCoords());
        }
        public List<Coord> GetNormalizedCoords()
        {
            return GetNormalizedCoords(Center);
        }
        public List<Coord> GetAllNeighbourCoords(List<Coord> coords)
        {
            List<Coord> neighbours = new List<Coord>();

            foreach (var coord in coords)
            {
                var results = Grid.GetNeighbourCoords(coord);
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

        protected override void MoveGridPosition(Coord oldCenter)
        {
            Grid.MoveTileObject(this, oldCenter);
        }

        protected override void RemoveFromGrid()
        {
            Grid.RemoveTileObject(this);
        }
    }
}