using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectVisual : MonoBehaviour
    {
        public abstract GridObject GetTileObject();
        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }
        public abstract void MoveToFront();
        public abstract void SetColor(Color color);
    }
}