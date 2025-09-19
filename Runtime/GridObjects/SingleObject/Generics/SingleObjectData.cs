using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class SingleObjectData<T, D, V> : GridObjectData<T, D, V>
        where T : SingleObject<T, D, V> where D : SingleObjectData<T, D, V> where V : SingleObjectVisual<T, D, V>
    {
        public Coord center = Coord.zero;

        public override int Layer
        {
            get
            {
                return layer;
            }
        }

        public override int TotalRotations
        {
            get
            {
                return totalRotations;
            }
        }
        [SerializeField, Min(1)] private int totalRotations = 1;
        [SerializeField] private int layer = default;

        public override bool IsValidPlacement(BaseGrid grid, Coord target, int rotation)
        {
            Coord normalized = center.Normalize(target, rotation);
            return grid.IsEmpty(Layer, normalized);
        }
        public override Dictionary<int, HashSet<Coord>> GetNormalizedCoordDatas(Coord target, int rotation)
        {
            return new Dictionary<int, HashSet<Coord>>() { { Layer, new HashSet<Coord>() { center + target } } };
        }
        public override List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord target, int rotation)
        {
            List<BoolCoord> boolCoords = new List<BoolCoord>();

            Coord normalized = center.Normalize(target, rotation);
            boolCoords.Add(new BoolCoord(normalized, grid.IsEmpty(Layer, normalized)));
            return boolCoords;
        }
    }
}