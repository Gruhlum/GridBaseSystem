using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem.Shapes
{
    [System.Serializable]
    public abstract class Shape
    {
        public abstract List<Coord> GetCoords(Coord center);
    }
}