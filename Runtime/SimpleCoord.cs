using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public struct SimpleCoord
    {
        public int x;
        public int y;

        public SimpleCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public SimpleCoord(Coord coord)
        {
            x = coord.x;
            y = coord.y;
        }

        public static explicit operator Coord(SimpleCoord simpleCoord)
        {
            return new Coord(simpleCoord.x, simpleCoord.y);
        }
    }
}