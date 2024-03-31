using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(menuName = "HexTecGames/MultiTileObjectData")]
    public class MultiTileObjectData : BaseTileObjectData
    {
        [SerializeField] private List<Coord> coords = default;
        public List<Sprite> sprites = new List<Sprite>();

        public int rotationCount;

        public override List<Coord> GetCoords()
        {
            var results = new List<Coord>();
            results.AddRange(coords);
            return results;
        }

        public override Sprite GetSprite()
        {
            return sprites[0];
        }
        public override bool IsDragable()
        {
            return false;
        }
    }
}