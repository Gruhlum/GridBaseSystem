using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[System.Serializable]
	public class TileObjectPlacementData
	{
		public TileObject tileObject;
		public bool IsBlocking;
		public bool isSaveZone;

        public TileObjectPlacementData(TileObject tileObject, bool isBlocking, bool isSaveZone)
        {
            this.tileObject = tileObject;
            this.IsBlocking = isBlocking;
            this.isSaveZone = isSaveZone;
        }
    }
}