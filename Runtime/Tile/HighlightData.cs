using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[System.Serializable]
	public class HighlightData
	{
        public float startDelay = default;
        public bool hasDuration = default;
        [DrawIf(nameof(hasDuration), true)] public float duration = 1;
        public float fadeIn = 0;
        public float fadeOut = 0;
        public Color color = Color.white;
    }
}