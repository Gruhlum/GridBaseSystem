using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(menuName = "HexTecGames/Grid/TileObjectData")]
    public class SimpleObjectData : TileObjectData<SimpleObject, SimpleObjectData, SimpleObjectVisual, SimpleObjectSaveData>
    {
        public override SimpleObject GenerateObject(BaseGrid grid, Coord coord, int rotation)
        {
            return new SimpleObject(this, grid, coord, rotation);
        }
    }
}