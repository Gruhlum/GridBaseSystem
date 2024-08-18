using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(menuName = "HexTecGames/TileObjectData")]
    public abstract class TileObjectData : GridObjectData
    {
        [SerializeField] private List<Coord> coords = default;
        [SerializeField] private List<Coord> saveZones = default;

        public TileObjectVisual VisualPrefab
        {
            get
            {
                return visualPrefab;
            }
            private set
            {
                visualPrefab = value;
            }
        }
        [SerializeField] private TileObjectVisual visualPrefab;

        public bool IsPassable
        {
            get
            {
                return false;
            }
        }
        public override bool IsDraggable
        {
            get
            {
                return false;
            }
        }


        public override bool IsValidCoord(Coord coord, BaseGrid grid)
        {
            return grid.IsTileEmpty(coord);
        }
        public CoordCollection GetNormalizedValidCoords(BaseGrid grid, Coord center, int rotation)
        {
            CoordCollection results = new CoordCollection();
            var saveZones = GetNormalizedSaveZones(grid, center, rotation);
            foreach (var coord in saveZones)
            {
                if (grid.IsTileBlocked(coord))
                {
                    results.invalidCoords.Add(coord);
                }
                else results.validCoords.Add(coord);
            }

            var coords = GetNormalizedCoords(grid, center, rotation);
            foreach (var coord in coords)
            {
                if (!grid.CanPlaceBuilding(coord))
                {
                    results.invalidCoords.Add(coord);
                }
                //else results.validCoords.Add(coord);
            }

            return results;
        }
        public override bool IsValidPlacement(Coord center, BaseGrid grid, int rotation)
        {
            if (!grid.CanPlaceBuilding(GetNormalizedCoords(grid, center, rotation)))
            {
                return false;
            }
            if (grid.IsTileBlocked(GetNormalizedSaveZones(grid, center, rotation)))
            {
                return false;
            }
            return true;
        }
        public List<Coord> GetNormalizedCoords(BaseGrid grid, Coord center, int rotation = 0)
        {
            return GetNormalizedAndRotatedCoords(grid, center, coords, rotation);
        }
        public List<Coord> GetCoords()
        {
            return new List<Coord>(coords);
        }
        public List<Coord> GetSaveZones()
        {
            return new List<Coord>(saveZones);
        }
        public List<Coord> GetNormalizedSaveZones(BaseGrid grid, Coord center, int rotation = 0)
        {
            return GetNormalizedAndRotatedCoords(grid, center, saveZones, rotation);
        }
        private List<Coord> GetNormalizedAndRotatedCoords(BaseGrid grid, Coord center, List<Coord> coords, int rotation)
        {
            var results = center.GetNormalizedCoords(coords);
            if (rotation != 0)
            {
                results = grid.GetRotatedCoords(center, results, rotation);
            }
            return results;
        }
    }
}