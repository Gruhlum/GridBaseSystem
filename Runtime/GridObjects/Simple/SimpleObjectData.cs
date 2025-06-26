using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(fileName = "New TileObject", menuName = "HexTecGames/Grid/TileObjectData")]
    public class SimpleObjectData : SingleObjectData<SimpleObject, SimpleObjectData, SimpleObjectVisual>, IGridObjectCreator
    {
        public GridObject CreateGridObject(BaseGrid grid, Coord coord, int rotation, GridObjectSaveData saveData = null)
        {
            return new SimpleObject(this, grid, coord, rotation, saveData);
        }
    }
}