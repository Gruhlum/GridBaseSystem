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
        [SerializeField] private MultiSpawner visualSpawner = default;

        private GridObjectVisual currentPrefab;
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
            GridObjectVisual prefab = placementData.Data.GetVisual();
            if (currentPrefab != prefab)
            {
                if (currentVisual != null)
                {
                    currentVisual.gameObject.SetActive(false);
                }
                currentPrefab = prefab;
                currentVisual = visualSpawner.Spawn(prefab);
                currentVisual.MoveToFront();
            }
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

        public void Rotate(int index)
        {
            rotation = index;

            if (activeData != null)
            {
                //sr.sprite = activeData.GetSprite(coord, grid, rotation);
                //SpriteData spriteData = activeData.GetSpriteData(rotation);
                //spriteOffset = spriteData.offset;
                //transform.eulerAngles = spriteData.rotation;
                //UpdatePosition();
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
            highlightSpawner.DeactivateAll();
            var results = data.Data.GetNormalizedValidCoords(grid, coord, rotation);

            foreach (var result in results)
            {
                highlightSpawner.SpawnHighlight(result.coord, result.valid ? validPlacementColor : invalidPlacementColor, false);
            }
        }
    }
}