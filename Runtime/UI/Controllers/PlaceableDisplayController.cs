using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class PlaceableDisplayController<T> : DisplayController<T> where T : GridObjectData
    {
        void Start()
        {
            DisplayItems();
        }

        protected void GridPlacementController_OnSelectedObjectChanged(GridObjectData data)
        {
            foreach (var display in displaySpawner.GetActiveInstances())
            {
                display.SetHighlight(data == display.Item);
            }
        }
    }
}