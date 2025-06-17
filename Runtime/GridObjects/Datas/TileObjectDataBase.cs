using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class TileObjectDataBase : GridObjectData
    {
        public abstract TileObjectBase GenerateTileObject(BaseGrid grid, Coord coord, int rotation);
    }
}