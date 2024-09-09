using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public class PreBuildInfo
    {
        public List<string> blockReasons = new List<string>();

        public bool IsBlocked
        {
            get
            {
                return blockReasons.Count > 0;
            }
        }

        public void AddBlockReason(string reason)
        {
            blockReasons.Add(reason);
        }

        public void PrintBlockReasons()
        {
            foreach (var reason in blockReasons)
            {
                Debug.Log(reason);
            }
        }
    }
}