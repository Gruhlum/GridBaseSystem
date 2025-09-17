using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(fileName = "New MultiObject", menuName = "HexTecGames/Grid/MultiObjectData")]
    public class MultiObjectData : MultiObjectData<MultiObject, MultiObjectData, MultiObjectVisual>, IGridObjectCreator
    {
        public GridObject CreateGridObject(BaseGrid grid, Coord coord, int rotation, GridObjectSaveData saveData = null)
        {
            MultiObject obj = new MultiObject(this, coord, rotation, saveData as MultiObjectSaveData);
            obj.AddToGrid(grid);
            return obj;
        }
    }
}