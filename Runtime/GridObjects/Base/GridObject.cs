using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
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
        public int Layer
        {
            get
            {
                return layer;
            }
            private set
            {
                layer = value;
            }
        }
        private int layer;

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
        public virtual int Rotation
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
            }
        }
        private int rotation;

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


        public GridObject(BaseGrid grid, GridObjectData data, Coord center, GridObjectSaveData saveData = null)
        {
            this.BaseData = data;
            this.Color = data.Color;
            this.Grid = grid;
            this.Center = center;
            this.Layer = data.Layer;

            if (saveData != null)
            {
                LoadSaveData(saveData);
            }
            if (grid != null)
            {
                AddToGrid(grid);
            }
        }
        protected abstract void AddToGrid(BaseGrid grid);

        public abstract void Remove();
        public abstract void Move(Coord target);
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
        public GridObjectVisual CreateVisual(BaseGrid grid)
        {
            return BaseData.CreateVisual(this, grid);
        }

        public virtual void LoadSaveData(GridObjectSaveData saveData) { }
        public abstract GridObjectSaveData GetSaveData();
        public Vector3 GetWorldPosition()
        {
            return Grid.CoordToWorldPosition(Center);
        }

        

        public override string ToString()
        {
            return $"{Name} ({Center})";
        }
    }
}