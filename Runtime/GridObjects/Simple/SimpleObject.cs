using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class SimpleObject : SingleObject<SimpleObject, SimpleObjectData, SimpleObjectVisual, SimpleObjectSaveData>
    {
        public SimpleObject(SimpleObjectData data, BaseGrid grid, Coord center, int rotation = 0) : base(data, grid, center, rotation)
        {
        }

        public override GridObjectSaveDataBase GetSaveData()
        {
            return new SimpleObjectSaveData(this);
        }

        
    }
}