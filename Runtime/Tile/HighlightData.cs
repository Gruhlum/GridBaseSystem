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
        public float fadeIn = 1;
        public float fadeOut = 1;
        public Color color = default;
    }
}