using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class MouseHighlighter : MonoBehaviour
	{
        [SerializeField] private TileHighlightSpawner highlightSpawner = default;
        [SerializeField] private GridEventSystem gridEventSys = default;

        [SerializeField] private HighlightData hoverData;
        [SerializeField] private HighlightData leftClickData;

        private void OnEnable()
        {
            gridEventSys.OnMouseClicked += GridEventSys_OnMouseClicked;
            gridEventSys.OnMouseHoverCoordChanged += GridEventSys_OnMouseHoverCoordChanged;
        }

        private void GridEventSys_OnMouseHoverCoordChanged(Coord coord)
        {
            highlightSpawner.DeactivateAll();
            if (!coord.isValid)
            {
                return;
            }
            highlightSpawner.Spawn().Activate(gridEventSys.Grid.CoordToWorldPoint(coord), hoverData);
        }

        private void GridEventSys_OnMouseClicked(Coord coord, int btn)
        {
            if (!coord.isValid)
            {
                return;
            }
            highlightSpawner.Spawn().Activate(gridEventSys.Grid.CoordToWorldPoint(coord), leftClickData);
        }
    }
}