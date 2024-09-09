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

        void Reset()
        {
            if (transform.parent != null)
            {
                gridEventSys = transform.parent.GetComponentInChildren<GridEventSystem>();
            }
            if (highlightSpawner == null)
            {
                highlightSpawner = new TileHighlightSpawner();
            }
            highlightSpawner.Parent = transform;
        }
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
            Vector3 position = gridEventSys.Grid.CoordToWorldPoint(coord);
            mouseClickHighlight.transform.position = position;
            highlightSpawner.Spawn().Activate(position, hoverData);
        }

        private void GridEventSys_OnMouseClicked(Coord coord, int btn)
        {
            if (!coord.isValid)
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