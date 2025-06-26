using HexTecGames.GridBaseSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public class GridObjectSaveData<T, D, V> : GridObjectSaveData
       where T : GridObject<T, D, V> where D : GridObjectData<T, D, V> where V : GridObjectVisual<T, D, V>
    {
        public D gridObjectData;

        public GridObjectSaveData(T gridObject) : base(gridObject)
        {
            this.gridObjectData = gridObject.Data;
        }
    }
}