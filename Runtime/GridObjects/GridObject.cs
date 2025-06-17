using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public abstract class GridObject
	{
        public BaseGrid Grid
        {
            get
            {
                return grid;
            }
            private set
            {
                grid = value;
            }
        }
        private BaseGrid grid;

        public Coord Center
        {
            get
            {
                return center;
            }
            protected set
            {
                center = value;
            }
        }
        private Coord center;

        public virtual Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        private Color color = Color.white;

        public string Name
        {
            get
            {
                if (BaseData == null)
                {
                    return "No Data";
                }
                return BaseData.name;
            }
        }

        public GridObjectData BaseData
        {
            get
            {
                return this.baseData;
            }
            private set
            {
                this.baseData = value;
            }
        }
        private GridObjectData baseData;

        //public event Action<GridObject, Sprite> OnSpriteChanged;
        //public event Action<GridObject, Color> OnColorChanged;

        public GridObject(BaseGrid grid, GridObjectData data, Coord center)
        {
            this.BaseData = data;
            this.Color = data.Color;
            this.Grid = grid;
            this.Center = center;
        }

        public Vector3 GetWorldPosition()
        {
            return Grid.CoordToWorldPosition(Center);
        }

        public abstract void Move(Coord coord);
        public abstract void Remove();
    }
}