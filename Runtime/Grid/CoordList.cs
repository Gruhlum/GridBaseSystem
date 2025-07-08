using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class CoordList
    {
        public int layer;
        public List<Coord> coords;

        public CoordList(int key, HashSet<Coord> hashSet)
        {
            this.layer = key;
            coords = hashSet.ToList();
        }

        internal HashSet<Coord> GenerateHashSet()
        {
            return coords.ToHashSet();
        }
    }
}