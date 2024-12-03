using System;
using Systems.Resources;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ResourceArea area))
            {
                
            }
        }
    }
}