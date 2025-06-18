using HexTecGames.Basics;
using HexTecGames.Basics.UI;
using HexTecGames.SoundSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectDataBase : ScriptableObject
    {
        [SerializeField] public List<CoordData> coordDatas = new List<CoordData>();

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



        public bool IsValidPlacement(BaseGrid grid, Coord coord, int rotation)
        {
            foreach (var coordData in coordDatas)
            {
                Coord normalized = coordData.coord.NormalizedAndRotated(coord, rotation);
                if (!grid.IsEmpty(coordData.layer, normalized))
                {
                    return false;
                }
            }
            return true;
        }
        public HashSet<CoordData> GetNormalizedCoordDatas(Coord center, int rotation)
        {
            HashSet<CoordData> results = new HashSet<CoordData>();

            foreach (var coordData in coordDatas)
            {
                results.Add(new CoordData(coordData.layer, coordData.coord.NormalizedAndRotated(center, rotation)));
            }
            return results;
        }
        public List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord center, int rotation)
        {
            List<BoolCoord> boolCoords = new List<BoolCoord>();

            foreach (var coordData in coordDatas)
            {
                Coord normalized = center;
                normalized.NormalizeAndRotate(coordData.coord, rotation);
                if (grid.IsEmpty(coordData.layer, coordData.coord))
                {
                    boolCoords.Add(new BoolCoord(normalized, true));
                }
                else boolCoords.Add(new BoolCoord(normalized, false));
            }
            return boolCoords;
        }
        public abstract GridObjectBase CreateGridObject(BaseGrid grid, Coord coord, int rotation);
        public  GridObjectVisualBase CreateVisual()
        {
            return CreateVisual(null, null);
        }
        public abstract GridObjectVisualBase CreateVisual(GridObjectBase obj, BaseGrid grid);
    }
}