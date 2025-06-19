using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(fileName = "New TileObject", menuName = "HexTecGames/Grid/TileObjectData")]
    public class SimpleObjectData : SingleObjectData<SimpleObject, SimpleObjectData, SimpleObjectVisual, SimpleObjectSaveData>
    {
        public override GridObjectBase CreateGridObject(BaseGrid grid, Coord coord, int rotation)
        {
            return new SimpleObject(this, grid, coord, rotation);
        }
    }
}