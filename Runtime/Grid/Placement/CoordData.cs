using HexTecGames.Basics;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public struct CoordData
    {
        public int layer;
        public Coord coord;


        public CoordData(int layer, Coord coord)
        {
            this.layer = layer;
            this.coord = coord;
        }

        public override string ToString()
        {
            return $"C:{coord} L:({layer})";
        }
    }
}