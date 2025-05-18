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
            protected set
            {
                center = value;
            }
        }
        private Coord center;

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
        public event Action<GridObject> OnRemoved;
        public event Action<GridObject, Coord, Coord> OnMoved;
        public event Action<GridObject, Color> OnColorChanged;

        public GridObject(BaseGrid grid, GridObjectData data, Coord center)
        {
            this.Grid = grid;
            this.BaseData = data;
            this.Center = center;
            this.Color = data.Color;
        }
        public GridObject(BaseGrid grid, GridObjectData data, Coord center, CustomSaveData customSaveData) : this(grid, data, center)
        {
            LoadCustomSaveData(customSaveData);
        }
        public virtual void Remove()
        {
            OnRemoved?.Invoke(this);
        }
        public virtual void Move(Coord target)
        {
            Coord oldCenter = center;
            center = target;
            MoveGridPosition(oldCenter);
            OnMoved?.Invoke(this, oldCenter, center);
        }

        protected abstract void MoveGridPosition(Coord oldCenter);
        public Vector3 GetWorldPosition()
        {
            return grid.CoordToWorldPosition(Center);
        }

        public virtual CustomSaveData GetCustomSaveData()
        { 
            return null; 
        }

        public virtual void LoadCustomSaveData(CustomSaveData data) 
        { }
    }
}