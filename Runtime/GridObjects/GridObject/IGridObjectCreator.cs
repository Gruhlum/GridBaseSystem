using HexTecGames.Basics;

namespace HexTecGames.GridBaseSystem
{
    public interface IGridObjectCreator
    {
        GridObject CreateGridObject(BaseGrid grid, Coord coord, int rotation, GridObjectSaveData saveData = null);
    }
}