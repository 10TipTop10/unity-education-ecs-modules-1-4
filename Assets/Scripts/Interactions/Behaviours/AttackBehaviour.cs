using UnityEngine;
using UnityEngine.AI;

public class AttackBehaviour : AIAgentBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private PlayerEntityBridge _playerEntityBridge;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackDelay = 1f;
    private double _nextAttackTime;


    public override float Evaluate()
    {
        if (!_playerEntityBridge.TryGetPlayerPosition(out Vector3 playerPosition)) return 0f;
        var distance = Vector3.Distance(playerPosition, transform.position);
        if (distance <= _attackDistance) return 1f;

        return 0f;
    }

    public override void Behave()
    {
        _navMeshAgent.ResetPath();

        double currentTime = Time.timeAsDouble;

        if (currentTime < _nextAttackTime) return;

        if (_playerEntityBridge.TryTakeDamage(_damage))
        {
            _nextAttackTime = currentTime + _attackDelay;
        }
    }
}
