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
            private set
            {
                data = value;
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
                if (rotation == value)
                {
                    return;
                }
                value = value.LoopValue(Grid.MaximumRotation);
                if (rotation == value)
                {
                    return;
                }
                rotation = value;
                OnRotated?.Invoke(this, rotation);
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

        public event Action<TileObject, int> OnRotated;



        public TileObject(BaseGrid grid, TileObjectData data, Coord center, int rotation = 0) : base(grid, data, center)
        {
            this.Data = data;
            //IsReplaceable = data.IsReplaceable;
            this.Rotation = rotation;
            //Sprite = data.GetSprite(center, grid, rotation);
            SetOccupyingTiles();
        }

        public void SetRotation(int rotation)
        {
            Rotation = rotation;
        }
        public void Rotate(int turns)
        {
            Rotation += turns;
        }
        public float DirectionToDegrees()
        {
            return DirectionToDegrees(Rotation);
        }
        public float DirectionToDegrees(int rotation)
        {
            return Grid.DirectionToDegrees(rotation);
        }
        public override void Remove()
        {
            base.Remove();
            RemoveOccupyingTiles();
            Grid.RemoveTileObject(this);
        }
        public Coord GetFacingCoord()
        {
            return Grid.GetDirectionCoord(Center, Rotation);
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
            if (Grid == null)
            {
                return;
            }
            foreach (var coord in Data.GetCoords())
            {
                Coord normalized = Center.NormalizedAndRotated(coord.coord, Rotation);
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

        //public SpriteData GetSpriteData()
        //{
        //    return Data.GetSpriteData(Center, Grid, Rotation);
        //}
        protected override void MoveGridPosition(Coord oldCenter)
        {
            List<PlacementCoord> newPlacements = new List<PlacementCoord>();
            List<PlacementCoord> oldPlacements = new List<PlacementCoord>();

            foreach (var placementCoord in Data.GetCoords())
            {
                placementCoord.NormalizeAndRotate(Center, Rotation);
                newPlacements.Add(placementCoord);
                //Debug.Log(placementCoord.test + " - " + newPlacements.Last().test);
                //Debug.Log(newPlacements[0].coord.ToString());
            }
            foreach (var placementCoord in Data.GetCoords())
            {
                placementCoord.NormalizeAndRotate(oldCenter, Rotation);
                //Check each new position if they are the same as an old position, if yes we can ignore it
                for (int i = newPlacements.Count - 1; i >= 0; i--)
                {
                    if (newPlacements[i].coord == placementCoord.coord && newPlacements[i].type == placementCoord.type)
                    {
                        //Debug.Log(newPlacements[i].coord + " - " + placementCoord.coord);
                        newPlacements.RemoveAt(i);
                    }
                    else oldPlacements.Add(placementCoord);
                }
            }

            //Debug.Log(newPlacements.Count + " - " + oldPlacements.Count);

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