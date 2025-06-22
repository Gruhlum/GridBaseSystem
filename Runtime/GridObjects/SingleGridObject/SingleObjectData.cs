using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class SingleObjectData<T, D, V, S> : GridObjectData<T, D, V, S>
        where T : SingleObject<T, D, V, S> where D : SingleObjectData<T, D, V, S> where V : SingleObjectVisual<T, D, V, S> where S : SingleObjectSaveData<T, D, V, S>
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
            HashSet<CoordData> results = new HashSet<CoordData>();
            results.Add(new CoordData(center.layer, (center.coord + target)));
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