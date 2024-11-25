using UnityEngine;

namespace Systems.Interaction
{
    public abstract class InteractionZone<TEnum> : MonoBehaviour where TEnum : System.Enum
    {
        public void Interact()
        {

        }
    }
}