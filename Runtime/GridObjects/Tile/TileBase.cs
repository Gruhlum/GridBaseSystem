using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class TileBase : GridObject
    {
        protected TileBase(BaseGrid grid, GridObjectData data, Coord center) : base(grid, data, center)
        {
        }
    }
}