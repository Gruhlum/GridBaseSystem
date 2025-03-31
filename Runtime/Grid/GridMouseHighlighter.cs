using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridMouseHighlighter : MonoBehaviour
    {
        [SerializeField] private GridEventSystem gridEventSys = default;
        [SerializeField] private TileHighlightSpawner highlightSpawner = default;
        [SerializeField] private TileHighlighter mouseClickHighlight = default;

        [SerializeField] private HighlightData hoverData;
        [SerializeField] private bool onlyShowOnTiles = false;

        private Coord lastCoord;

        private void Reset()
        {
            if (transform.parent != null)
            {
                gridEventSys = transform.parent.GetComponentInChildren<GridEventSystem>();
            }
            if (highlightSpawner == null)
            {
                highlightSpawner = new TileHighlightSpawner();
            }
            highlightSpawner.Grid = GetComponentInParent<BaseGrid>();
            highlightSpawner.Parent = transform;
        }
        private void OnEnable()
        {
            gridEventSys.OnMouseClicked += GridEventSys_OnMouseClicked;
            gridEventSys.OnMouseHoverCoordChanged += GridEventSys_OnMouseHoverCoordChanged;
            MouseController.OnPointerOverUIChanged += MouseController_OnPointerOverUIChanged;
        }
        private void OnDisable()
        {
            gridEventSys.OnMouseClicked -= GridEventSys_OnMouseClicked;
            gridEventSys.OnMouseHoverCoordChanged -= GridEventSys_OnMouseHoverCoordChanged;
            MouseController.OnPointerOverUIChanged -= MouseController_OnPointerOverUIChanged;
        }
        private void MouseController_OnPointerOverUIChanged(bool overUI)
        {
            if (overUI)
            {
                highlightSpawner.DeactivateAll();
            }
            else DisplayHighlight(lastCoord);
        }
        private void GridEventSys_OnMouseHoverCoordChanged(Coord coord)
        {
            lastCoord = coord;
            DisplayHighlight(coord);
        }
        private void DisplayHighlight(Coord coord)
        {
            highlightSpawner.DeactivateAll();
            if (onlyShowOnTiles && !coord.isValid)
            {
                return;
            }
            if (MouseController.IsPointerOverUI)
            {
                return;
            }
            Vector3 position = gridEventSys.Grid.CoordToWorldPoint(coord);
            mouseClickHighlight.transform.position = position;
            highlightSpawner.Spawn().Activate(position, hoverData);
        }
        private void GridEventSys_OnMouseClicked(Coord coord, int btn)
        {
            if (onlyShowOnTiles && !coord.isValid)
            {
                return;
            }
            mouseClickHighlight.Activate(gridEventSys.Grid.CoordToWorldPoint(coord));
            StartCoroutine(DisableAfterMouseUp());
        }

        private IEnumerator DisableAfterMouseUp()
        {
            while (Input.GetMouseButton(0))
            {
                yield return null;
            }
            mouseClickHighlight.Deactivate();
        }
    }
}