using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class Tile : GridObject
    {
        public int X
        {
            get
            {
                return Center.x;
            }
        }
        public int Y
        {
            get
            {
                return Center.y;
            }
        }

        public TileData Data
        {
            get
            {
                return data;
            }
            private set
            {
                data = value;
            }
        }
        private TileData data;

        private List<TileObjectPlacementData> placementDatas = new List<TileObjectPlacementData>();

        //public Tile(int x, int y, BaseGrid grid) : base(new Coord(x, y), grid)
        //{

        //}
        public Tile(Coord coord, BaseGrid grid, TileData tileData) : base(coord, grid, tileData)
        {
            this.Data = tileData;
            UpdateSprite();
        }
        public void UpdateSprite()
        {
            Sprite = Data.GetSprite(Center, Grid);
        }
        public void RemoveAllTileObjects()
        {
            placementDatas.Clear();
        }
        public void RemoveBlockingTileObject()
        {
            TileObjectPlacementData placementData = placementDatas.Find(x => x.IsBlocking);
            placementDatas.Remove(placementData);
        }
        public void RemoveTileObject(TileObject tileObj)
        {
            TileObjectPlacementData placementData = placementDatas.Find(x => x.tileObject == tileObj);
            placementDatas.Remove(placementData);
        }
        public void AddTileObject(TileObjectPlacementData placmentData)
        {
            placementDatas.Add(placmentData);
        }
        public bool TryGetTileObject(out TileObject tileObj)
        {
            TileObjectPlacementData placementData = placementDatas.Find(x => x.IsBlocking);
            if (placementData != null)
            {
                tileObj = placementData.tileObject;
                return true;
            }
            tileObj = null;
            return false;
        }
        public TileObject GetTileObject()
        {
            TileObjectPlacementData placementData = placementDatas.Find(x => x.IsBlocking);
            if (placementData != null)
            {
                return placementData.tileObject;
            }
            else return null;
        }
        public bool IsSaveZone()
        {
            foreach (var placementData in placementDatas)
            {
                if (placementData.isSaveZone)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsBlocked()
        {
            foreach (var placementData in placementDatas)
            {
                if (placementData.IsBlocking)
                {
                    return true;
                }
            }
            return false;
        }
        protected override void MoveGridPosition(Coord oldCenter)
        {
            throw new NotImplementedException();
        }

        protected override void RemoveFromGrid()
        {
            RemoveAllTileObjects();
            Grid.RemoveTile(this);
        }

        public override string ToString()
        {
            return $"({X}, {Y}) {Data.Name}";
        }
    }
}