using UnityEngine;
using UnityEngine.AI;

public class WaitBehaviour : AIAgentBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    public override float Evaluate()
    {
        return 0.1f;
    }

    public override void Behave()
    {
        _navMeshAgent.ResetPath();
    }
}
