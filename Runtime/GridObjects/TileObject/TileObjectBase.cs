using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class TileObjectBase : GridObject
    {
        public abstract int Rotation
        {
            get;
            protected set;
        }

        public TileObjectDataBase TileObjectDataBase
        {
            get
            {
                return this.tileObjectDataBase;
            }
            private set
            {
                this.tileObjectDataBase = value;
            }
        }

        private TileObjectDataBase tileObjectDataBase;

        protected TileObjectBase(TileObjectDataBase data, BaseGrid grid, Coord center) : base(grid, data, center)
        {
            this.TileObjectDataBase = data;
        }

        public GridObjectVisual CreateVisual(BaseGrid grid)
        {
            return TileObjectDataBase.CreateVisual(this, grid);
        }

        public abstract TileObjectSaveDataBase GetSaveData();
    }
}