using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public abstract class GridObject<T> : GridObjectBase where T : GridObject<T>
	{
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnColorChanged?.Invoke(this as T, color);
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
        public event Action<T> OnRemoved;
        public event Action<T, Coord, Coord> OnMoved;
        public event Action<T, Color> OnColorChanged;

        public GridObject(BaseGrid grid, GridObjectData data, Coord center) : base(grid, center)
        {
            this.BaseData = data;
            this.Color = data.Color;
        }
        public virtual void Remove()
        {
            OnRemoved?.Invoke(this as T);
        }
        public virtual void Move(Coord target)
        {
            Coord oldCenter = Center;
            Center = target;
            MoveGridPosition(oldCenter);
            OnMoved?.Invoke(this as T, oldCenter, Center);
        }

        //public virtual CustomSaveData GetCustomSaveData()
        //{ 
        //    return null; 
        //}

        //public virtual void LoadCustomSaveData(CustomSaveData data) 
        //{ }
    }
}