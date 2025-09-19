using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using HexTecGames.GridBaseSystem;
using UnityEngine;

namespace HexTecGames.GridBaseSystem.Classes
{
    [System.Serializable]
    public class MultiObject : MultiObject<MultiObject, MultiObjectData, MultiObjectVisual>
    {
        public MultiObject(MultiObjectData data, Coord center, int rotation = 0, MultiObjectSaveData saveData = null) : base(data, center, rotation, saveData)
        {
        }
    }
}