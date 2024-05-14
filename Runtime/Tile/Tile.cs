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

        //public Tile(int x, int y, BaseGrid grid) : base(new Coord(x, y), grid)
        //{

        //}
        public Tile(Coord coord, BaseGrid grid, TileData tileData) : base(coord, grid)
        {
            this.Data = tileData;
            Sprite = tileData.Sprite;
        }

        protected override void MoveGridPosition(Coord oldCenter)
        {
            throw new NotImplementedException();
        }

        protected override void RemoveFromGrid()
        {
            Grid.RemoveTile(this);
        }
    }
}