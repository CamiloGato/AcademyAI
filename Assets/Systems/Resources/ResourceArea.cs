using UnityEngine;

namespace Systems.Resources
{
    public abstract class ResourceArea : MonoBehaviour
    {
        public ResourceConfiguration configuration;

        public void Load()
        {
            print("Resources Area Loading with: " + configuration.resourceName);
            SetUp();
        }

        public void UnLoad()
        {
            print("Resources Area Unload");
            Close();
        }
        
        protected abstract void SetUp();
        protected abstract void Close();
    }
}