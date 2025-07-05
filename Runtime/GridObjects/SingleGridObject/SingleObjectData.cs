using System.Collections.Generic;
using HexTecGames.Basics;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class SingleObjectData<T, D, V> : GridObjectData<T, D, V>
        where T : SingleObject<T, D, V> where D : SingleObjectData<T, D, V> where V : SingleObjectVisual<T, D, V>
    {
        public CoordData center = new CoordData(0, Coord.zero);

        public override int Layer
        {
            get
            {
                return center.layer;
            }
        }

        public override bool IsValidPlacement(BaseGrid grid, Coord target, int rotation)
        {
            Coord normalized = center.coord + target;
            normalized.Rotate(center.coord, rotation);
            return grid.IsEmpty(Layer, normalized);
        }
        public override HashSet<CoordData> GetNormalizedCoordDatas(Coord target, int rotation)
        {
            HashSet<CoordData> results = new HashSet<CoordData>
            {
                new CoordData(center.layer, center.coord + target)
            };
            return results;
        }
        public override List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord target, int rotation)
        {
            List<BoolCoord> boolCoords = new List<BoolCoord>();

            Coord normalized = center.coord + target;
            normalized.Rotate(center.coord, rotation);
            boolCoords.Add(new BoolCoord(normalized, grid.IsEmpty(center.layer, normalized)));
            return boolCoords;
        }
    }
}