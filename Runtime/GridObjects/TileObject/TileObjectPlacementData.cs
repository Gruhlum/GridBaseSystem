using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[System.Serializable]
	public struct TileObjectPlacementData
	{
		public TileObject tileObject;
		public CoordType type;

        public TileObjectPlacementData(TileObject tileObject, CoordType type)
        {
            this.tileObject = tileObject;
            this.type = type;
        }
    }
}