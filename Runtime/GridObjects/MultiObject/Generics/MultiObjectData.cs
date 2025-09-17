using System;
using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class MultiObjectData<T, D, V> : GridObjectData<T, D, V>
        where T : MultiObject<T, D, V> where D : MultiObjectData<T, D, V> where V : MultiObjectVisual<T, D, V>
    {
        public Dictionary<int, HashSet<Coord>> coordDatas;
        [SerializeField] public List<Sprite> sprites = new List<Sprite>();

        public override int TotalRotations
        {
            get
            {
                return sprites.Count;
            }
        }
        public override int Layer
        {
            get
            {
                return layer;
            }
        }
        [SerializeField] private int layer = default;

        [SerializeField] private List<CoordList> coordLists = new List<CoordList>();

#if UNITY_EDITOR
        public void SerializeDictionary()
        {
            coordLists.Clear();
            if (coordDatas == null)
            {
                return;
            }
            foreach (var coordData in coordDatas)
            {
                coordLists.Add(new CoordList(coordData.Key, coordData.Value));
            }
            EditorUtility.SetDirty(this);
        }
#endif
        public void LoadDictionary()
        {
            if (coordLists == null)
            {
                return;
            }
            if (coordDatas == null)
            {
                coordDatas = new Dictionary<int, HashSet<Coord>>();
            }
            foreach (var coordList in coordLists)
            {
                coordDatas.Add(coordList.layer, coordList.GenerateHashSet());
            }
        }

        public bool HasCoordData(int layer, int x, int y)
        {
            if (coordDatas == null)
            {
                LoadDictionary();
            }
            if (coordDatas.TryGetValue(layer, out HashSet<Coord> coords))
            {
                var result = coords.Any(coord => coord.x == x && coord.y == y);
                return result;
            }
            else return false;
        }

        private void ForEachNormalizedCoord(Coord target, int rotation, Action<int, Coord> action)
        {
            if (coordDatas == null)
            {
                LoadDictionary();
            }

            foreach (var coordData in coordDatas)
            {
                foreach (var coord in coordData.Value)
                {
                    Coord normalized = target + coord;
                    normalized.Rotate(target, rotation);
                    action(coordData.Key, normalized);
                }
            }
        }
        public override bool IsValidPlacement(BaseGrid grid, Coord target, int rotation)
        {
            bool isValid = true;

            ForEachNormalizedCoord(target, rotation, (layer, normalized) =>
            {
                if (!grid.IsEmpty(layer, normalized))
                {
                    isValid = false;
                }
            });

            return isValid;
        }
        public override Dictionary<int, HashSet<Coord>> GetNormalizedCoordDatas(Coord target, int rotation)
        {
            Dictionary<int, HashSet<Coord>> results = new Dictionary<int, HashSet<Coord>>();

            ForEachNormalizedCoord(target, rotation, (layer, normalized) =>
            {
                if (!results.TryGetValue(layer, out var set))
                {
                    set = new HashSet<Coord>();
                    results[layer] = set;
                }
                set.Add(normalized);
            });

            return results;
        }
        public override List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord target, int rotation)
        {
            List<BoolCoord> boolCoords = new List<BoolCoord>();

            ForEachNormalizedCoord(target, rotation, (layer, normalized) =>
            {
                bool isValid = grid.IsEmpty(layer, normalized);
                boolCoords.Add(new BoolCoord(normalized, isValid));
            });

            return boolCoords;
        }
    }
}