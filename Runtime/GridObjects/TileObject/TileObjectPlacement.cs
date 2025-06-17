using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[System.Serializable]
	public struct TileObjectPlacement
	{
		public TileObjectBase tileObject;
		public CoordType type;

        public TileObjectPlacement(TileObjectBase tileObject, CoordType type)
        {
            this.tileObject = tileObject;
            this.type = type;
        }

        public override string ToString()
        {
            return $"{tileObject.Name} {type}";
        }
    }
}