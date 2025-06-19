using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class GridObjectBase
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
        public abstract int Rotation
        {
            get;
            protected set;
        }

        public GridObjectDataBase BaseData
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
        private GridObjectDataBase baseData;


        public GridObjectBase(BaseGrid grid, GridObjectDataBase data, Coord center)
        {
            this.BaseData = data;
            this.Color = data.Color;
            this.Grid = grid;
            this.Center = center;

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
        public GridObjectVisualBase CreateVisual(BaseGrid grid)
        {
            return BaseData.CreateVisual(this, grid);
        }

        public abstract GridObjectSaveDataBase GetSaveData();
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