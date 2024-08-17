using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class PlacementGhost : MonoBehaviour
	{
        [SerializeField] private SpriteRenderer sr = default;

        [SerializeField] private BaseGrid grid = default;
        [SerializeField] private TileHighlightSpawner highlightSpawner = default;
        private GridObjectData activeData;
        private Coord coord;
        private int rotation;

        [SerializeField] private Color validPlacementColor = Color.green;
        [SerializeField] private Color invalidPlacementColor = Color.red;

        public void SetSprite(Sprite sprite, Color color)
        {
            sr.sprite = sprite;
            sr.color = color;
        }
        public void Activate(GridObjectData data, Coord center)
        {
            activeData = data;            
            SetSprite(data.Sprite, data.Color);
            rotation = 0;
            Activate(center);
        }
        public void Activate(Coord coord)
        {
            this.coord = coord;
            transform.position = grid.CoordToWorldPoint(coord);
            gameObject.SetActive(true);
            if (activeData is TileObjectData tileObjectData)
            {
                UpdatePlacementArea(tileObjectData);
            }
        }
        public void Deactivate()
        {
            gameObject.SetActive(false);
            highlightSpawner.DeactivateAll();
        }
        public void Rotate(int index)
        {
            rotation = index;
            transform.eulerAngles = new Vector3(0, 0, -90 * index);
        }
        public void UpdatePlacementArea()
        {
            if (activeData != null && activeData is TileObjectData tileObjData)
            {
                UpdatePlacementArea(tileObjData);
            }           
        }
        private void UpdatePlacementArea(TileObjectData data)
        {
            var results = data.GetNormalizedValidCoords(grid, coord, rotation);
            //if (results.invalidCoords.Count > 0)
            //{
            //    sr.color = invalidPlacementColor;
            //}
            //else sr.color = data.Color;
            highlightSpawner.SpawnHighlights(results.validCoords, validPlacementColor, grid);
            highlightSpawner.SpawnHighlights(results.invalidCoords, invalidPlacementColor, grid, false);
        }
    }
}