using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class GridCreator : MonoBehaviour
	{
		[SerializeField] private BaseGrid grid = default;

		public bool createOnStart = true;
		public int width = 20;
		public int height = 20;


        void Start()
        {
			if (createOnStart) grid.GenerateGrid(width, height);
        }
    }
}