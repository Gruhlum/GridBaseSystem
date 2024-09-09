using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public struct PlacementCoord
    {
        public Coord coord;
        public CoordType type;

        //public void Normalize(Coord center)
        //{
        //    coord.Normalized(center);
        //}
        //public void Rotate(Coord center, int rotation)
        //{
        //    coord.Rotated(center, rotation);
        //}
        //public void NormalizeAndRotate(Coord center, int rotation)
        //{
        //    coord.NormalizedAndRotated(center, rotation);
        //}
    }
}