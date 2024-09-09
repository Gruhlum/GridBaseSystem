using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public int Rotation
        {
            get
            {
                return rotation;
            }
            private set
            {
                rotation = value;
            }
        }
        private int rotation;

        public virtual bool IsReplaceable
        {
            get
            {
                return isReplaceable;
            }
            set
            {
                isReplaceable = value;
            }
        }
        private bool isReplaceable;


        public List<Tile> occupyingTiles = new List<Tile>();

        public TileObject(Coord center, BaseGrid grid, TileObjectData data, int rotation) : base(center, grid, data)
        {
            this.data = data;
            IsReplaceable = data.IsReplaceable;
            this.Rotation = rotation;
            Sprite = data.GetSprite(center, grid, rotation);
            SetOccupyingTiles();
        }

        public void Rotate(int rotation)
        {
            Rotation = rotation;
            Sprite = Data.GetSprite(Center, Grid, Rotation);
        }

        public override void Remove()
        {
            base.Remove();
            RemoveOccupyingTiles();
        }

        private void RemoveOccupyingTiles()
        {
            foreach (var tile in occupyingTiles)
            {
                tile.RemoveTileObject(this);
            }
        }
        private void SetOccupyingTiles()
        {
            foreach (var coord in Data.GetCoords())
            {
                Coord normalized = Center.NormalizeAndRotate(coord.coord, Rotation);
                var tile = Grid.GetTile(normalized);
                tile.AddTileObject(this, coord.type);
                occupyingTiles.Add(tile);
            }
        }
        //public List<Coord> GetNormalizedCoords(Coord center, int rotation)
        //{
        //    return Data.GetNormalizedCoords(Grid, center, rotation);
        //}
        //public List<Coord> GetNormalizedCoords()
        //{
        //    return GetNormalizedCoords(Center, Rotation);
        //}
        //public List<Coord> GetAllNeighbourCoords(List<Coord> coords)
        //{
        //    List<Coord> neighbours = new List<Coord>();

        //    foreach (var coord in coords)
        //    {
        //        var results = Grid.GetNeighbourCoords(coord);
        //        foreach (var result in results)
        //        {
        //            if (!coords.Contains(result) && !neighbours.Contains(result))
        //            {
        //                neighbours.Add(result);
        //            }
        //        }
        //    }
        //    return neighbours;
        //}

        public SpriteData GetSpriteData()
        {
            return Data.GetSpriteData(Center, Grid, Rotation);
        }
        protected override void MoveGridPosition(Coord oldCenter)
        {
            List<PlacementCoord> newPlacements = new List<PlacementCoord>();
            List<PlacementCoord> oldPlacements = new List<PlacementCoord>();

            foreach (var coord in Data.GetCoords())
            {
                coord.coord.NormalizedAndRotated(Center, Rotation);
                newPlacements.Add(coord);
            }
            foreach (var coord in Data.GetCoords())
            {
                coord.coord.NormalizedAndRotated(oldCenter, Rotation);
                for (int i = newPlacements.Count - 1; i >= 0; i--)
                {
                    if (newPlacements[i].coord == coord.coord && newPlacements[i].type == coord.type)
                    {
                        newPlacements.RemoveAt(i);
                    }
                    else oldPlacements.Add(coord);
                }
            }
            foreach (var placement in oldPlacements)
            {
                Tile tile = Grid.GetTile(placement.coord);
                tile.RemoveTileObject(this);
            }
            foreach (var placement in newPlacements)
            {
                Tile tile = Grid.GetTile(placement.coord);
                tile.AddTileObject(this, placement.type);
                occupyingTiles.Add(tile);
            }
        }
    }
}