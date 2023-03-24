using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Tyrants
{
    [RequireComponent(typeof(NavMeshAgent), typeof(NetworkTransformReliable))]
    public class TyrantMoveLogic : NetworkBehaviour
    {
        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public IEnumerator MoveToTarget(Transform targetTransform, float stopDistance, Action onArrive)
        {
            _agent.destination = targetTransform.position;
            while ((transform.position - targetTransform.position).sqrMagnitude > stopDistance)
            {
                _agent.destination = targetTransform.position;
                yield return null;
            }
            onArrive?.Invoke();
        }

        public bool FindTarget(out NetworkIdentity target, float radius, LayerMask targetMask, QueryTriggerInteraction triggerInteraction)
        {
            target = null;
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetMask, triggerInteraction);
            if (colliders.Length == 0) return false;
            target = colliders[0].GetComponentInParent<NetworkIdentity>();
            if (target == null) return false;
            return true;
        }
    }
}