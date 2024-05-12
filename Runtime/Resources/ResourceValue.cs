using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[System.Serializable]
	public class ResourceValue
	{
        public ResourceType Data
        {
            get
            {
                return data;
            }
            private set
            {
                data = value;
            }
        }
        [SerializeField] private ResourceType data;

        public virtual int Value
        {
            get
            {
                return value;
            }
            set
            {              
                this.value = value;
            }
        }
        [SerializeField] private int value;
    }
}