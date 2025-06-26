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
            SimpleObject obj = new SimpleObject(this, coord, rotation, saveData);
            obj.AddToGrid(grid);
            return obj;
        }
    }
}