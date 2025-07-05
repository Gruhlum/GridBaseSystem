using System.Collections.Generic;

namespace HexTecGames.GridBaseSystem
{
    public interface ICost
    {
        bool IsAffordable(List<Resource> resources);
        void SubtractResources(List<Resource> resources);

        ResourceValue GetCost();
    }
}