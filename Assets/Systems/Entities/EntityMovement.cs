using System;
using UnityEngine;
using UnityEngine.AI;

namespace Systems.Entities
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EntityMovement : MonoBehaviour
    {
        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        public void MoveTo(Vector3 destination)
        {
            _agent.isStopped = false;
            _agent.destination = destination;
        }

        public void StopMoving()
        {
            _agent.isStopped = true;
        }
    }
}