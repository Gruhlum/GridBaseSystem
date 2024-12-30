using HexTecGames.GridBaseSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public class TileObjectSaveData : GridObjectSaveData
    {
        public int rotation;

        public TileObjectSaveData(TileObject gridObj) : base(gridObj)
        {
            rotation = gridObj.Rotation;
        }
    }
}