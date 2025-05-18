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
        [SerializeReference, SubclassSelector] public CustomSaveData customSaveData;

        public GridObjectSaveData(GridObject gridObj)
        {
            position = gridObj.Center;
            customSaveData = gridObj.GetCustomSaveData();
        }
    }
}