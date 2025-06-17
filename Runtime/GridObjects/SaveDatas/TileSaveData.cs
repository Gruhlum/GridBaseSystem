using HexTecGames.GridBaseSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public class TileSaveData : GridObjectSaveData
    {
        public TileData tileData;

        public TileSaveData(Tile tile) : base(tile)
        {
            tileData = tile.Data;
        }
    }
}