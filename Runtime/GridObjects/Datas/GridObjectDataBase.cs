using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectDataBase : ScriptableObject
    {
        public abstract bool IsValidCoord(BaseGrid grid, Coord coord, int rotation = 0);
        public abstract List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord center, int rotation);

        public abstract IGridObjectVisual GetVisual();
    }
}