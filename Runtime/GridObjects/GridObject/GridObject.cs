using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObject<T, D, V, S> : GridObject
        where T : GridObject<T, D, V, S> where D : GridObjectData<T, D, V, S> where V : GridObjectVisual<T, D, V, S> where S : GridObjectSaveData<T, D, V, S>
    {
        public D Data
        {
            get
            {
                return data;
            }
            private set
            {
                data = value;
            }
        }
        private D data;

        public override int Rotation
        {
            get
            {
                return base.Rotation;
            }
            protected set
            {
                base.Rotation = value;
                OnRotated?.Invoke(this as T, Rotation);
            }
        }
        public override Color Color
        {
            get
            {
                return base.Color;
            }

            set
            {
                base.Color = value;
                OnColorChanged?.Invoke(this as T, Color);
            }
        }
        public virtual bool IsReplaceable
        {
            get
            {
                return isReplaceable;
            }
            set
            {
                isReplaceable = value;
            }
        }
        private bool isReplaceable;

        public event Action<T, int> OnRotated;
        public event Action<T> OnRemoved;
        public delegate void MoveEvent(T gridObj, Coord start, Coord target);
        public event MoveEvent OnMoved;
        public event Action<T, Color> OnColorChanged;

        public GridObject(D data, BaseGrid grid, Coord center, int rotation = 0) : base(grid, data, center)
        {
            this.Data = data;
            this.Rotation = rotation;
        }


        public sealed override void Remove()
        {
            RemoveFromGrid();
            OnRemoved?.Invoke(this as T);
        }
        protected abstract void RemoveFromGrid();


        public sealed override void Move(Coord targetCoord)
        {
            Coord currentCoord = Center;
            Center = targetCoord;
            Move(currentCoord, targetCoord);
            OnMoved?.Invoke(this as T, currentCoord, targetCoord);
        }
        protected abstract void Move(Coord currentCoord, Coord targetCoord);
    }
}