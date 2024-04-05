using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(menuName = "HexTecGames/MultiTileObjectData")]
    public abstract class MultiTileObjectData : BaseTileObjectData
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
        public override bool IsDraggable
        {
            get
            {
                return false;
            }
        }
    }
}