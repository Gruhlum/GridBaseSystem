using System.Collections;
using System.Collections.Generic;
using HexTecGames.GridBaseSystem;
using UnityEngine;

namespace HexTecGames.GridBaseSystem.Classes
{
    [System.Serializable]
    public class MultiObjectSaveData : MultiObjectSaveData<MultiObject, MultiObjectData, MultiObjectVisual>
    {
        public MultiObjectSaveData(MultiObject gridObj) : base(gridObj)
        {
        }
    }
}