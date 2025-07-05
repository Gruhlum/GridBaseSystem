using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class ResourceController : MonoBehaviour
    {
        [SerializeField] private List<Resource> resources = default;

        private void OnValidate()
        {
            if (resources != null)
            {
                foreach (Resource resource in resources)
                {
                    resource.OnValidate();
                }
            }
        }

        public Resource GetResource(ResourceValue resource)
        {
            return GetResource(resource.Data);
        }
        public Resource GetResource(ResourceType type)
        {
            return resources.Find(x => x.Data == type);
        }
        public List<Resource> GetResources()
        {
            return resources;
        }
    }
}