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

        [SerializeField] private MultiSpawner ghostVisualSpawner = default;

        private PlacementData activeData;
        private Coord coord;
        private int rotation;
        private Vector2 spriteOffset;

        [SerializeField] private Color validPlacementColor = Color.green;
        [SerializeField] private Color invalidPlacementColor = Color.red;

        private Transform activeGhostT;

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
            if (placementData.GhostPrefab != null)
            {
                if (activeGhostT != null)
                {
                    activeGhostT.gameObject.SetActive(false);
                }
                activeGhostT = ghostVisualSpawner.Spawn(placementData.GhostPrefab);
            }
            rotation = 0;
            Activate(center);
        }
        public void Activate(Coord coord)
        {
            isActive = true;
            this.coord = coord;
            transform.position = grid.CoordToWorldPoint(coord);
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

        private void UpdatePosition()
        {
            if (activeGhostT != null)
            {
                activeGhostT.localPosition = spriteOffset;
            }
        }

        public void Rotate(int index)
        {
            rotation = index;

            if (activeData != null)
            {
                //sr.sprite = activeData.GetSprite(coord, grid, rotation);
                //SpriteData spriteData = activeData.GetSpriteData(rotation);
                //spriteOffset = spriteData.offset;
                //transform.eulerAngles = spriteData.rotation;
                UpdatePosition();
            }
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
            var results = data.Data.GetNormalizedValidCoords(grid, coord, rotation);

            foreach (var result in results)
            {
                highlightSpawner.SpawnHighlight(result.coord, result.valid ? validPlacementColor : invalidPlacementColor, grid);
            }
        }
    }
}