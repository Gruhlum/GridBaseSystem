using HexTecGames.GridBaseSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public class GridObjectSaveData<T, D, V, S> : GridObjectSaveData
       where T : GridObject<T, D, V, S> where D : GridObjectData<T, D, V, S> where V : GridObjectVisual<T, D, V, S> where S : GridObjectSaveData<T, D, V, S>
    {
        public D gridObjectData;

        public GridObjectSaveData(T gridObject) : base(gridObject)
        {
            this.gridObjectData = gridObject.Data;
        }
    }
}