using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class SimpleObject : TileObject<SimpleObject, SimpleObjectData, SimpleObjectVisual, SimpleObjectSaveData>
    {
        public SimpleObject(SimpleObjectData data, BaseGrid grid, Coord center, int rotation = 0) : base(data, grid, center, rotation)
        {
        }

        protected override SimpleObjectSaveData GetTileObjectSaveData()
        {
            return new SimpleObjectSaveData(this);
        }
    }
}