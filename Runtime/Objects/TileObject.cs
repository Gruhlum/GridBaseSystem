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

        public List<Coord> GetNormalizedCoords(Coord center)
        {
            var coords = data.GetCoords();
            for (int i = 0; i < coords.Count; i++)
            {
                coords[i] += center;
            }
            return coords;
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