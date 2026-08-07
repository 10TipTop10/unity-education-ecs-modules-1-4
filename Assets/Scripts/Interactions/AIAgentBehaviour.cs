using UnityEngine;

public abstract class AIAgentBehaviour : MonoBehaviour
{
    public abstract float Evaluate();

    public abstract void Behave();
}