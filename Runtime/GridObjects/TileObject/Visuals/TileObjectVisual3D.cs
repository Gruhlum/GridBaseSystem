using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class TileObjectVisual3D<T, D, V, S> : TileObjectVisual<T, D, V, S>
       where T : TileObject<T, D, V, S> where D : TileObjectData<T, D, V, S> where V : TileObjectVisual<T, D, V, S> where S : TileObjectSaveData<T, D, V, S>
    {
        public override void MoveToFront()
        {
            throw new System.NotImplementedException();
        }

        protected override void Rotate(int rotation)
        {
            throw new System.NotImplementedException();
        }
    }
}