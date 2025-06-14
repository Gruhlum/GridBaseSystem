using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class Tile : GridObject<Tile>
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

        public bool IsUnblocked
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

        private List<TileObjectPlacement> placementDatas = new List<TileObjectPlacement>();

        public event Action<Tile, TileObjectPlacement> OnTileObjectAdded;
        public event Action<Tile, TileObjectPlacement> OnTileObjectRemoved;


        //public Tile(int x, int y, BaseGrid grid) : base(new Coord(x, y), grid)
        //{

        //}
        public Tile(BaseGrid grid, TileData tileData, Coord coord) : base(grid, tileData, coord)
        {
            this.Data = tileData;
        }
        private void CheckTileState()
        {
            IsPassable = true;
            IsUnblocked = true;

            foreach (var item in placementDatas)
            {
                if (item.type == CoordType.Blocking)
                {
                    IsPassable = false;
                    IsUnblocked = false;
                    return;
                }
                if (item.type == CoordType.Passable)
                {
                    IsUnblocked = false;
                }
            }
        }
        public void RemoveAllTileObjects()
        {
            placementDatas.Clear();
            IsPassable = true;
            IsUnblocked = true;
        }

        public void RemoveTileObject(TileObject tileObj)
        {
            TileObjectPlacement placementData = placementDatas.Find(x => x.tileObject == tileObj);
            if (placementData.tileObject == null || !placementDatas.Remove(placementData))
            {
                //Debug.Log("FAIL: " + tileObj.Name + " - " + Center);
                for (int i = 0; i < placementDatas.Count; i++)
                {
                    //TileObjectPlacement pData = placementDatas[i];
                    //Debug.Log(i + " - " + pData + " Target: " + tileObj.Name + " this Center: " + Center);
                }
                //Debug.Log("Something is wrong: " + tileObj.Name + " - " + tileObj.Center);
            }
            CheckTileState();
            OnTileObjectRemoved?.Invoke(this, placementData);
        }

        public void AddTileObject(TileObject tileObj, CoordType type)
        {
            var placementData = new TileObjectPlacement(tileObj, type);
            placementDatas.Add(placementData);
            if (type == CoordType.Blocking)
            {
                IsPassable = false;
                IsUnblocked = false;
            }
            else IsUnblocked = false;

            OnTileObjectAdded?.Invoke(this, placementData);
        }
        //public bool TryGetTileObject(out TileObject tileObj)
        //{
        //    if (placementDatas == null || placementDatas.Count <= 0)
        //    {
        //        tileObj = null;
        //        return false;
        //    }
        //    foreach (var data in placementDatas)
        //    {
        //        if (data.tileObject == tileObj)
        //        {

        //        }
        //    }
        //    tileObj = placementDatas[0].tileObject;
        //    return true;
        //}
        public List<TileObjectPlacement> GetTileObject()
        {
            if (placementDatas == null)
            {
                return null;
            }
            return new List<TileObjectPlacement>(placementDatas);
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
            return $"({X}, {Y}) {Data.name}";
        }

        public virtual void LoadSaveData(TileSaveData saveData)
        {
            
        }
    }
}