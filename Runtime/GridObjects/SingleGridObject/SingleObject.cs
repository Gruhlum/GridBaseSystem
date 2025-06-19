using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class SingleObject<T, D, V, S> : GridObject<T, D, V, S>
        where T : SingleObject<T, D, V, S> where D : SingleObjectData<T, D, V, S> where V : SingleObjectVisual<T, D, V, S> where S : SingleObjectSaveData<T, D, V, S>
    {
        protected SingleObject(D data, BaseGrid grid, Coord center, int rotation = 0) : base(data, grid, center, rotation)
        {
        }

        protected override void RemoveFromGrid()
        {
            Grid.RemoveGridObject(Layer, Center, this);
        }
        protected override void AddToGrid(BaseGrid grid)
        {
            grid.AddGridObject(Layer, Center, this);
        }

        protected override void Move(Coord currentCoord, Coord targetCoord)
        {
            Grid.MoveGridObject(Layer, currentCoord, targetCoord, this);
        }
    }
}