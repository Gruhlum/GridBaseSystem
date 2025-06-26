using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class MultiObjectData<T, D, V> : GridObjectData<T, D, V>
        where T : MultiObject<T, D, V> where D : MultiObjectData<T, D, V> where V : MultiObjectVisual<T, D, V>
    {
        [SerializeField] public List<CoordData> coordDatas = new List<CoordData>() { new CoordData(0, Coord.zero) };

        public override int Layer
        {
            get
            {
                return coordDatas[0].layer;
            }
        }

        public override bool IsValidPlacement(BaseGrid grid, Coord target, int rotation)
        {
            foreach (var coordData in coordDatas)
            {
                Coord normalized = coordData.coord + target;
                normalized.Rotate(coordData.coord, rotation);
                if (!grid.IsEmpty(coordData.layer, normalized))
                {
                    return false;
                }
            }
            return true;
        }

        public override HashSet<CoordData> GetNormalizedCoordDatas(Coord target, int rotation)
        {
            HashSet<CoordData> results = new HashSet<CoordData>();

            foreach (var coordData in coordDatas)
            {
                results.Add(new CoordData(coordData.layer, (coordData.coord + target)));
            }
            return results;
        }
        public override List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord target, int rotation)
        {
            List<BoolCoord> boolCoords = new List<BoolCoord>();

            foreach (var coordData in coordDatas)
            {
                Coord normalized = target + coordData.coord;
                normalized.Rotate(coordData.coord, rotation);
                if (grid.IsEmpty(coordData.layer, normalized))
                {
                    boolCoords.Add(new BoolCoord(normalized, true));
                }
                else boolCoords.Add(new BoolCoord(normalized, false));
            }
            return boolCoords;
        }
    }
}