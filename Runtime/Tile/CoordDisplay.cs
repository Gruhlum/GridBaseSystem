using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class CoordDisplay : MonoBehaviour
	{
        [SerializeField] private SpriteRenderer sr = default;

        public Coord Coord
        {
            get
            {
                return coord;
            }
            private set
            {
                coord = value;
            }
        }
        private Coord coord;


        void Reset()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void Setup(Coord coord, Vector3 position)
		{
            this.Coord = coord;
            transform.position = position;
            gameObject.name = coord.ToString();
		}
        public void SetColor(Color color)
        {
            if (sr != null)
            {
                sr.color = color;
            }
        }
	}
}