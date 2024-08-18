using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class TileObject : GridObject
    {
        public TileObjectData Data
        {
            get
            {
                return data;
            }
        }
        private TileObjectData data;

        public TileObject(Coord center, BaseGrid grid, TileObjectData data) : base(center, grid, data)
        {
            this.data = data;
            Sprite = data.GetSprite(center, grid, Rotation);
        }
        public List<Coord> GetNormalizedSafeZones()
        {
            return Data.GetNormalizedSaveZones(Grid, Center, Rotation);
        }
        public List<Coord> GetNormalizedCoords(Coord center)
        {
            return Data.GetNormalizedCoords(Grid, center, Rotation);
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