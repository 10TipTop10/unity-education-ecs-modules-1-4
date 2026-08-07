using System.Collections.Generic;
using UnityEngine;

public class BehaviourManager : MonoBehaviour
{
    [SerializeField] private List<AIAgentBehaviour> AIAgentBehaviours = new ();
    private AIAgentBehaviour _activeBehaviour;

    private void Update()
    {
        GetBehaviorWithHighScore();

        if (_activeBehaviour == null) return;

        _activeBehaviour.Behave();
    }

    private void GetBehaviorWithHighScore()
    {
        float bestScore = float.NegativeInfinity;
        _activeBehaviour = null;
        foreach (var behaviour in AIAgentBehaviours)
        {
            if(behaviour == null) continue;

            float currentScore = behaviour.Evaluate();
            if(currentScore > bestScore)
            {
                bestScore = currentScore;
                _activeBehaviour = behaviour;
            }
        }
    }
}
