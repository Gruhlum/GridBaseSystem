using HexTecGames.GridBaseSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public class TileObjectSaveData : GridObjectSaveData
    {
        public TileObjectData tileObjectData;
        public int rotation;

        public TileObjectSaveData(TileObject tileObject) : base(tileObject)
        {
            this.tileObjectData = tileObject.Data;
            rotation = tileObject.Rotation;
        }
    }
}