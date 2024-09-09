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

        public bool IsPassable
        {
            get
            {
                return isPassable;
            }
            private set
            {
                isPassable = value;
            }
        }
        private bool isPassable = true;

        public bool IsBuildable
        {
            get
            {
                return isBuildable;
            }
            private set
            {
                isBuildable = value;
            }
        }
        private bool isBuildable = true;

        public bool IsEmpty
        {
            get
            {
                return placementDatas.Count == 0;
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
            Sprite = this.Data.Sprite;
        }
        //public void UpdateSprite()
        //{
        //    Sprite = Data.GetSprite(Center, Grid, 0);
        //}
        private void CheckForPassable()
        {
            foreach (var item in placementDatas)
            {
                if (item.type != CoordType.Passable)
                {
                    IsPassable = false;
                    return;
                }
            }
            IsPassable = true;
        }
        public void RemoveAllTileObjects()
        {
            placementDatas.Clear();
            IsPassable = true;
        }

        public void RemoveTileObject(TileObject tileObj)
        {
            TileObjectPlacementData placementData = placementDatas.Find(x => x.tileObject == tileObj);
            placementDatas.Remove(placementData);
            CheckForPassable();
            CheckForBlocking();
        }
        private void CheckForBlocking()
        {
            foreach (var placementData in placementDatas)
            {
                if (placementData.type != CoordType.Blocking)
                {
                    IsBuildable = false;
                }
            }
            IsBuildable = true;
        }
        public void AddTileObject(TileObject tileObj, CoordType type)
        {
            placementDatas.Add(new TileObjectPlacementData(tileObj, type));
            if (type == CoordType.Passable)
            {
                IsPassable = false;
            }
            else IsBuildable = false;
        }
        public bool TryGetTileObject(out TileObject tileObj)
        {
            if (placementDatas == null || placementDatas.Count <= 0)
            {
                tileObj = null;
                return false;
            }
            tileObj = placementDatas[0].tileObject;
            return true;
        }
        public List<TileObjectPlacementData> GetTileObject()
        {
            if (placementDatas == null)
            {
                return null;
            }
            return new List<TileObjectPlacementData>(placementDatas);
        }

        protected override void MoveGridPosition(Coord oldCenter)
        {
            throw new NotImplementedException();
        }

        public override void Remove()
        {
            RemoveAllTileObjects();
            base.Remove();
        }

        public override string ToString()
        {
            return $"({X}, {Y}) {Data.Name}";
        }
    }
}