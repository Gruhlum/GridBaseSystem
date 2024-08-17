using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class CoordCollection
    {
        public List<Coord> validCoords = new List<Coord>();
        public List<Coord> invalidCoords = new List<Coord>();
    }
}