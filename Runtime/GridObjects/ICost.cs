using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{	
	public interface ICost
	{
		public bool IsAffordable(List<Resource> resources);
		public void SubtractResources(List<Resource> resources);

		public ResourceValue GetCost();
	}
}