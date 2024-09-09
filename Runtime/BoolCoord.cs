using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public struct BoolCoord
    {
        public Coord coord;
        public bool valid;

        public BoolCoord(Coord coord, bool valid)
        {
            this.coord = coord;
            this.valid = valid;
        }

        public override string ToString()
        {
            return $"{coord}: {valid}";
        }
    }
}