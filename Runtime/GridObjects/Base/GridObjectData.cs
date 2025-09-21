using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectData : ScriptableObject
    {
        public Color Color
        {
            get
            {
                return color;
            }
            private set
            {
                color = value;
            }
        }
        [SerializeField] private Color color = Color.white;

        public abstract int TotalRotations { get; }
        public abstract int Layer
        {
            get;
        }
        [Space]
        [SerializeField] private bool requiresSubLayer = default;
        [SerializeField, DrawIf(nameof(requiresSubLayer), true)] private int requiredLayer = default;

        protected bool IsValidCoord(BaseGrid grid, int layer, Coord normalized)
        {
            if (requiresSubLayer && grid.IsEmpty(requiredLayer, normalized))
            {
                return false;
            }
            if (!grid.IsEmpty(layer, normalized))
            {
                return false;
            }
            return true;
        }
        public abstract GridObjectVisual GetVisualPrefab();
        public abstract bool IsValidPlacement(BaseGrid grid, Coord target, int rotation);
        public abstract Dictionary<int, HashSet<Coord>> GetNormalizedCoordDatas(Coord target, int rotation);
        public abstract List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord target, int rotation);
    }
}