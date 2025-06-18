using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class SimpleObjectSaveData : GridObjectSaveData<SimpleObject, SimpleObjectData, SimpleObjectVisual, SimpleObjectSaveData>
    {
        public SimpleObjectSaveData(SimpleObject tileObject) : base(tileObject)
        {
        }
    }
}