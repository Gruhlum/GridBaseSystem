using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class TileObject<T, D, V, S> : TileObjectBase
        where T : TileObject<T, D, V, S> where D : TileObjectData<T, D, V, S> where V : TileObjectVisual<T, D, V, S> where S : TileObjectSaveData<T, D, V, S>
    {
        public D Data
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
        private D data;

        public override int Rotation
        {
            get
            {
                return rotation;
            }
            protected set
            {
                if (rotation == value)
                {
                    return;
                }
                value = value.WrapDirection(Grid.MaximumRotation);
                if (rotation == value)
                {
                    return;
                }
                rotation = value;
                OnRotated?.Invoke(this as T, rotation);
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

        public event Action<T, int> OnRotated;

        public event Action<T> OnRemoved;
        public event Action<T, Coord, Coord> OnMoved;
        public event Action<T, Color> OnColorChanged;

        public TileObject(D data, BaseGrid grid, Coord center, int rotation = 0) : base(data, grid, center)
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
            RemoveOccupyingTiles();
            Grid.RemoveTileObject(this);
            OnRemoved?.Invoke(this as T);
        }
        public Coord GetFacingCoord()
        {
            return Grid.GetDirectionCoord(Rotation) + Center;
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
            foreach (var coord in Data.coords)
            {
                Coord normalized = Center.NormalizedAndRotated(coord.coord, Rotation);
                var tile = Grid.GetTile(normalized);
                tile.AddTileObject(this, coord.type);
                occupyingTiles.Add(tile);
            }
        }
        public override void Move(Coord target)
        {
            Coord oldCenter = Center;
            Center = target;
            MoveGridPosition(oldCenter);
            OnMoved?.Invoke(this as T, oldCenter, Center);
        }
        protected void MoveGridPosition(Coord oldCenter)
        {
            occupyingTiles[0].RemoveTileObject(this);
            Tile tile = Grid.GetTile(Center);
            occupyingTiles[0] = tile;
            tile.AddTileObject(this, Data.coords[0].type);

            // -----------------------
            // THIS NEEDS TO BE REENABLED FOR MULTI TILE OBJECTS TO WORK
            // -----------------------

            //List<PlacementCoord> newPlacements = new List<PlacementCoord>();
            //List<PlacementCoord> oldPlacements = new List<PlacementCoord>();

            //foreach (var placementCoord in Data.GetCoords())
            //{
            //    placementCoord.NormalizeAndRotate(Center, Rotation);
            //    newPlacements.Add(placementCoord);
            //    //Debug.Log(placementCoord.test + " - " + newPlacements.Last().test);
            //    //Debug.Log(newPlacements[0].coord.ToString());
            //}
            //foreach (var placementCoord in Data.GetCoords())
            //{
            //    placementCoord.NormalizeAndRotate(oldCenter, Rotation);
            //    //Check each new position if they are the same as an old position, if yes we can ignore it
            //    for (int i = newPlacements.Count - 1; i >= 0; i--)
            //    {
            //        if (newPlacements[i].coord == placementCoord.coord && newPlacements[i].type == placementCoord.type)
            //        {
            //            //Debug.Log(newPlacements[i].coord + " - " + placementCoord.coord);
            //            newPlacements.RemoveAt(i);
            //        }
            //        else oldPlacements.Add(placementCoord);
            //    }
            //}

            ////Debug.Log(newPlacements.Count + " - " + oldPlacements.Count);

            //foreach (var placement in oldPlacements)
            //{
            //    Tile tile = Grid.GetTile(placement.coord);
            //    tile.RemoveTileObject(this);
            //}
            //foreach (var placement in newPlacements)
            //{
            //    Tile tile = Grid.GetTile(placement.coord);
            //    tile.AddTileObject(this, placement.type);
            //    occupyingTiles.Add(tile);
            //}
            //Debug.Log(Name + " old: " + oldCenter + " new: " + occupyingTiles[0].Center);
        }

        protected abstract S GetTileObjectSaveData();

        public sealed override TileObjectSaveDataBase GetSaveData()
        {
            return GetTileObjectSaveData();
        }
    }
}