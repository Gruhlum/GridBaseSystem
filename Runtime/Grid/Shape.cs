using System.Collections.Generic;
using HexTecGames.Basics;

namespace HexTecGames.GridBaseSystem.Shapes
{
    [System.Serializable]
    public abstract class Shape
    {
        public abstract List<Coord> GetCoords(Coord center);
    }
}