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

        public GridObjectSaveData(GridObjectBase gridObj)
        {
            position = gridObj.Center;
        }
    }
}