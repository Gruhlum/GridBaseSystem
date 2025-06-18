using System.Collections;
using System.Collections.Generic;
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

        private HashSet<CoordData> coordDatas = new HashSet<CoordData>();

        //public event Action<GridObject, Sprite> OnSpriteChanged;
        //public event Action<GridObject, Color> OnColorChanged;

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

        private void AddToGrid(BaseGrid grid)
        {
            coordDatas = BaseData.GetNormalizedCoordDatas(Center, Rotation);
            grid.AddGridObject(coordDatas, this);
        }
        public virtual void Remove()
        {
            grid.RemoveGridObject(coordDatas, this);
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

        public virtual void Move(Coord oldCoord, Coord targetCoord)
        {
            HashSet<CoordData> newCoords = BaseData.GetNormalizedCoordDatas(targetCoord, Rotation);
            HashSet<CoordData> dataToRemove = new HashSet<CoordData>(coordDatas);
            HashSet<CoordData> dataToAdd = new HashSet<CoordData>();

            foreach (var data in newCoords)
            {
                if (dataToRemove.Contains(data))
                {
                     dataToRemove.Remove(data);
                }
                else dataToAdd.Add(data);
            }

            foreach (var remove in dataToRemove)
            {
                grid.RemoveGridObject(remove, this);
            }
            foreach (var add in dataToAdd)
            {
                grid.AddGridObject(add, this);
            }
        }
    }
}