using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private CombatMovementMode _combatMovementMode = CombatMovementMode.SlowDown;
    [SerializeField, Range(0f, 1f)] private float _combatSpeed = 0.5f;
    [SerializeField] private EnemyRoute _route;
    [SerializeField] private NavMeshAgent _agent;
    private float _normalSpeed;
    private bool _routeCompleted;
    private int _currentPointIndex = 0;

    public bool RouteCompleted => _routeCompleted;

    private void Awake()
    {
        _normalSpeed = _agent.speed;
    }

    private void Start()
    {
        _agent.SetDestination(_route.GetPoint(_currentPointIndex).position);
    }

    private void Update()
    {
        if (_routeCompleted) return;
        if (_agent.pathPending) return;
        if (_agent.remainingDistance > _agent.stoppingDistance + 0.05f) return;

        _currentPointIndex++;
        if (_currentPointIndex < _route.Count)
        {
            _agent.SetDestination(_route.GetPoint(_currentPointIndex).position);
        }
        else
        {
            _routeCompleted = true;
        }
    }
    public void SetRoute(EnemyRoute route)
    {
        _route = route;
    }

    public void SetCombatState(bool inCombat)
    {
        if(!_agent.enabled || !_agent.isOnNavMesh) return;

        bool shouldStop = inCombat && _combatMovementMode == CombatMovementMode.Stop;

        _agent.isStopped = shouldStop;

        bool shouldSlowDown = inCombat && _combatMovementMode == CombatMovementMode.SlowDown;

        _agent.speed = shouldSlowDown ? _normalSpeed * _combatSpeed : _normalSpeed; 
    }
}
