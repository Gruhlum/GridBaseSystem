using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class PlacementGhost : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;
        [SerializeField] private TileHighlightSpawner highlightSpawner = default;
        [SerializeField] private SpriteRenderer sr = default;

        private GridObjectVisual currentVisual;

        private PlacementData activeData;
        private Coord coord;
        private int rotation;

        [SerializeField] private Color validPlacementColor = Color.green;
        [SerializeField] private Color invalidPlacementColor = Color.red;

        private bool isHiding;
        private bool isActive;

        protected virtual void Reset()
        {
            grid = transform.GetComponentInParent<BaseGrid>();
            if (highlightSpawner == null)
            {
                highlightSpawner = new TileHighlightSpawner();
            }
            highlightSpawner.Parent = transform;
            highlightSpawner.Grid = grid;
        }

        public void Activate(PlacementData placementData, Coord center)
        {
            activeData = placementData;
            if (currentVisual != null)
            {
                currentVisual.Deactivate();
            }

            currentVisual = placementData.Data.CreateVisual();
            currentVisual.transform.SetParent(transform);
            currentVisual.SetColor(placementData.GetColor());
            currentVisual.transform.localPosition = Vector3.zero;
            currentVisual.MoveToFront();

            rotation = 0;
            Activate(center);
        }
        public void Activate(Coord coord)
        {
            isActive = true;
            this.coord = coord;
            transform.position = grid.CoordToWorldPosition(coord);

            if (isHiding)
            {
                return;
            }
            gameObject.SetActive(true);
            UpdatePlacementArea(activeData);
        }
        public void Deactivate()
        {
            isActive = false;
            gameObject.SetActive(false);
            isHiding = false;
            highlightSpawner.DeactivateAll();
        }

        public void Show(bool show)
        {
            if (!isActive)
            {
                return;
            }
            gameObject.SetActive(show);
            isHiding = !show;
        }

        public void UpdatePlacementArea(Coord coord, int rotation)
        {
            this.coord = coord;
            this.rotation = rotation;
            transform.position = grid.CoordToWorldPosition(coord);
            UpdatePlacementArea();
        }
        public void UpdatePlacementArea()
        {
            if (activeData != null)
            {
                UpdatePlacementArea(activeData);
            }
        }
        private void UpdatePlacementArea(PlacementData data)
        {
            highlightSpawner.DeactivateAll();
            var results = data.Data.GetNormalizedValidCoords(grid, coord, rotation);

            foreach (var result in results)
            {
                highlightSpawner.SpawnHighlight(result.coord, result.valid ? validPlacementColor : invalidPlacementColor, false);
            }
        }
    }
}