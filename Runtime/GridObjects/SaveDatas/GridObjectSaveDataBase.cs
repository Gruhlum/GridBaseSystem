using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class GridObjectSaveDataBase
    {
        public Coord position;
        public int rotation;
        public GridObjectDataBase data;

        public GridObjectSaveDataBase(GridObjectBase gridObj)
        {
            this.position = gridObj.Center;
            this.rotation = gridObj.Rotation;
            this.data = gridObj.BaseData;
        }
    }
}