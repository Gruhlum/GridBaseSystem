using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class TileObjectSaveDataBase : GridObjectSaveData
    {
        public TileObjectDataBase data;
        public int rotation;

        protected TileObjectSaveDataBase(TileObjectBase tileObjBase) : base(tileObjBase)
        {
            this.data = tileObjBase.TileObjectDataBase;
            this.rotation = tileObjBase.Rotation;
        }
    }
}