using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public struct PlacementCoord
    {
        public Coord coord;
        public CoordType type;

        public void Normalize(Coord center)
        {
            coord.Normalize(center);
        }
        public void Rotate(Coord center, int rotation)
        {
            coord.Rotate(center, rotation);
        }
        public void NormalizeAndRotate(Coord center, int rotation)
        {
            //Debug.Log("Before: " + coord.ToString());
            coord.NormalizeAndRotate(center, rotation);
            //Debug.Log("After: " + coord.ToString() + " - C: " + center.ToString());
        }

        public override string ToString()
        {
            return coord.ToString() + " " + type.ToString();
        }
    }
}