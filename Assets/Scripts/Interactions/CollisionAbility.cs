using UnityEngine;
using System.Collections.Generic;
using Unity.Entities;

public class CollisionAbility : MonoBehaviour
{
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private List<CollisionAction> _actions = new();

    public string TargetTag => _targetTag;
    public IReadOnlyList<CollisionAction> Actions => _actions;

    public void ExecuteActions(Entity targetEntity, EntityManager entityManager, EntityCommandBuffer commandBuffer)
    {
        foreach (var action in Actions)
        {
            if (action == null) continue;

            action.Execute(targetEntity, entityManager, commandBuffer);
        }
    }
}
