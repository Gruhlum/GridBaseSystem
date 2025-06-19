using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObject<T, D, V, S> : GridObjectBase
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
                OnRotated?.Invoke(this as T, rotation);
            }
        }
        private int rotation;

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
        public event Action<T, Coord, Coord> OnMoved;
        public event Action<T, Color> OnColorChanged;

        public GridObject(D data, BaseGrid grid, Coord center, int rotation = 0) : base(grid, data, center)
        {
            this.Data = data;
            this.Layer = data.Layer;
            this.Rotation = rotation;
        }

        
        public float DirectionToDegrees()
        {
            return DirectionToDegrees(Rotation);
        }
        public float DirectionToDegrees(int rotation)
        {
            return Grid.DirectionToDegrees(rotation);
        }
        public sealed override void Remove()
        {
            OnRemoved?.Invoke(this as T);
        }
        protected abstract void RemoveFromGrid();


        public sealed override void Move(Coord targetCoord)
        {
            Coord lastCoord = Center;
            Move(lastCoord, targetCoord);
            OnMoved?.Invoke(this as T, lastCoord, targetCoord);
        }
        protected abstract void Move(Coord currentCoord, Coord targetCoord);
    }
}