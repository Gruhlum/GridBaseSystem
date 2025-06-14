using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectVisual : MonoBehaviour, IGridObjectVisual
    {
        public abstract void MoveToFront();
    }
}