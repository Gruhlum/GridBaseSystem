using System;
using System.Collections;
using System.Collections.Generic;
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

        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                if (sprite == value)
                {
                    return;
                }
                sprite = value;
                OnSpriteChanged?.Invoke(this, sprite);
            }
        }
        private Sprite sprite;

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

        public event Action<GridObject, Sprite> OnSpriteChanged;
        public event Action<GridObject, Color> OnColorChanged;
        public event Action<GridObject> OnRemoved;
        public event Action<GridObject> OnMoved;

        public int Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                Sprite = baseData.GetSprite(center, grid, Rotation);
                OnMoved?.Invoke(this);
            }
        }
        private int rotation;


        private GridObjectData baseData;

        public GridObject(Coord center, BaseGrid grid, GridObjectData data)
        {
            this.Grid = grid;
            this.Center = center;
            this.baseData = data;
            Color = data.Color;
        }

        public virtual void Remove()
        {
            RemoveFromGrid();
            OnRemoved?.Invoke(this);
        }
        protected abstract void RemoveFromGrid();
        public virtual void Move(Coord target)
        {
            Coord oldCenter = center;
            center = target;
            MoveGridPosition(oldCenter);
            OnMoved?.Invoke(this);
        }
        protected abstract void MoveGridPosition(Coord oldCenter);
        public Vector3 GetWorldPosition()
        {
            return grid.CoordToWorldPoint(Center);
        }
        public SpriteData GetSpriteData()
        {
            return baseData.GetSpriteData(Rotation);
        }

    }
}