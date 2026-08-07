using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : AIAgentBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private PlayerEntityBridge _playerEntityBridge;
    [SerializeField] private float _detectionDistance = 6f;
    public override float Evaluate()
    {
        if (!_playerEntityBridge.TryGetPlayerPosition(out Vector3 playerPosition)) return 0f;
        var distance = Vector3.Distance(playerPosition, transform.position);
        if (distance <= _detectionDistance) return 0.5f;

        return 0f;
    }

    public override void Behave()
    {
        if (!_playerEntityBridge.TryGetPlayerPosition(out Vector3 playerPosition)) return;
        _navMeshAgent.SetDestination(playerPosition);
    }
}