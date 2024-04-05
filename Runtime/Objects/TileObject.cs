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
        protected BaseTileObjectData data;


        public Coord Center
        {
            get
            {
                return center;
            }
        }
        private Coord center;


        public event Action<TileObject> OnRemoved;
        public event Action<TileObject> OnMoved;

        public TileObject(Coord center, BaseGrid grid, BaseTileObjectData data)
        {
            this.grid = grid;
            this.data = data;
            this.center = center;
          
            grid.AddTileObject(this);
        }

        public void Remove()
        {
            grid.RemoveTileObject(this);
            OnRemoved?.Invoke(this);
        }
        public void Move(Coord target)
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

        public Vector3 GetWorldPosition()
        {
            return grid.CoordToWorldPoint(Center);
        }
    }
}