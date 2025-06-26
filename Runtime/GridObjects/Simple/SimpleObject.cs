using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class SimpleObject : SingleObject<SimpleObject, SimpleObjectData, SimpleObjectVisual>
    {
        public SimpleObject(SimpleObjectData data, Coord center, int rotation = 0, GridObjectSaveData saveData = null) 
            : base(data, center, rotation, saveData)
        {
        }

        public override GridObjectSaveData GetSaveData()
        {
            return new SimpleObjectSaveData(this);
        }

        
    }
}