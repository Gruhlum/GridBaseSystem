using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class GridObjectSaveData
    {
        public Coord position;
        public int rotation;
        public GridObjectData data;

        public GridObjectSaveData(GridObject gridObj)
        {
            this.position = gridObj.Center;
            this.rotation = gridObj.Rotation;
            this.data = gridObj.BaseData;
        }
    }
}