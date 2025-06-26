using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public interface IGridObjectCreator
    {
        public GridObject CreateGridObject(BaseGrid grid, Coord coord, int rotation, GridObjectSaveData saveData = null);
    }
}