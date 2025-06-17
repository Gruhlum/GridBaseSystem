using HexTecGames.GridBaseSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public class TileObjectSaveData<T, D, V, S> : TileObjectSaveDataBase
       where T : TileObject<T, D, V, S> where D : TileObjectData<T, D, V, S> where V : TileObjectVisual<T, D, V, S> where S : TileObjectSaveData<T, D, V, S>
    {
        public D tileObjectData;

        public TileObjectSaveData(T tileObject) : base(tileObject)
        {
            this.tileObjectData = tileObject.Data;
        }
    }
}