namespace HexTecGames.GridBaseSystem
{
    public enum Neighbour { TopLeft, Top, TopRight, Right, DownRight, Down, DownLeft, Left }
    public enum CoordType { Blocking, Passable }
    public enum ColorType { FromData, Custom }
    public enum GridType { Rect, Hexagon }

    // Blocking: No other objects can occupy this space
    // Passable: Objects can move through it but no Buildings can be placed

    // A 'Blockable' coord can not be placed on either
    // A 'Passable' coord can overlap with other passable coords
}