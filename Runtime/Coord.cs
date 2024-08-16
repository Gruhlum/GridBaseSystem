using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public struct Coord
    {
        public int x;
        public int y;
        [HideInInspector] public bool isValid;

        public readonly static Coord zero = new Coord(0, 0);
        public readonly static Coord up = new Coord(0, 1);
        public readonly static Coord down = new Coord(0, -1);
        public readonly static Coord left = new Coord(-1, 0);
        public readonly static Coord right = new Coord(1, 0);
        public readonly static Coord one = new Coord(1, 1);

        public Coord(int x, int y) : this(x, y, true)
        {
        }
        public Coord(int x, int y, bool isValid) 
        {
            this.x = x;
            this.y = y;
            this.isValid = isValid;
        }
        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void Set(Coord coord)
        {
            Set(coord.x, coord.y);
        }

        public override bool Equals(object obj)
        {
            return obj is Coord coord &&
                   this.x == coord.x &&
                   this.y == coord.y;
        }
        public override int GetHashCode()
        {
            return System.HashCode.Combine(x, y);
        }

        public List<Coord> GetRotatedCoords(List<Coord> coords)
        {
            List<Coord> results = new List<Coord>();
            foreach (var coord in coords)
            {
                results.Add(this + coord);
            }
            return results;
        }
        public List<Coord> GetNormalizedCoords(List<Coord> coords)
        {
            List<Coord> results = new List<Coord>();
            foreach (var coord in coords)
            {
                results.Add(this + coord);
            }
            return results;
        }
        public static bool operator ==(Coord coord1, Coord coord2)
        {
            if (coord1.x != coord2.x)
            {
                return false;
            }
            if (coord1.y != coord2.y)
            {
                return false;
            }
            return true;
        }
        public static bool operator !=(Coord coord1, Coord coord2)
        {
            return !(coord1 == coord2);
        }
        public static Coord operator +(Coord coord1, Coord coord2)
        {
            coord1.x += coord2.x;
            coord1.y += coord2.y;
            return coord1;
        }
        public static Coord operator -(Coord coord1, Coord coord2)
        {
            coord1.x -= coord2.x;
            coord1.y -= coord2.y;
            return coord1;
        }
        public static Coord operator *(Coord coord1, Coord coord2)
        {
            coord1.x *= coord2.x;
            coord1.y *= coord2.y;
            return coord1;
        }
        public static Coord operator /(Coord coord1, Coord coord2)
        {
            coord1.x /= coord2.x;
            coord1.y /= coord2.y;
            return coord1;
        }
        public static Coord operator *(Coord coord1, int value)
        {
            coord1.x *= value;
            coord1.y *= value;
            return coord1;
        }
        public static Coord operator /(Coord coord1, int value)
        {
            coord1.x /= value;
            coord1.y /= value;
            return coord1;
        }
        public static Coord operator +(Coord coord1, Vector2 vector)
        {
            coord1.x += Mathf.RoundToInt(vector.x);
            coord1.y += Mathf.RoundToInt(vector.y);
            return coord1;
        }
        public static Coord operator -(Coord coord1, Vector2 vector)
        {
            coord1.x -= Mathf.RoundToInt(vector.x);
            coord1.y -= Mathf.RoundToInt(vector.y);
            return coord1;
        }
        public override string ToString()
        {
            return $"({x}), ({y})";
        }
    }
}