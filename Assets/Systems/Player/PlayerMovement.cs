using UnityEngine;
using UnityEngine.AI;

namespace Systems.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMovement : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Move(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
        }
    }
}