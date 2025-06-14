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

        protected GridObjectBase(BaseGrid grid, Coord center)
        {
            this.Grid = grid;
            this.Center = center;
        }

        public Vector3 GetWorldPosition()
        {
            return Grid.CoordToWorldPosition(Center);
        }
        protected abstract void MoveGridPosition(Coord oldCenter);
    }
}