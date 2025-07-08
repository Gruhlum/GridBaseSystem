using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class MultiObjectSaveData : MultiObjectSaveData<MultiObject, MultiObjectData, MultiObjectVisual>
    {
        public MultiObjectSaveData(MultiObject gridObj) : base(gridObj)
        {
        }
    }
}