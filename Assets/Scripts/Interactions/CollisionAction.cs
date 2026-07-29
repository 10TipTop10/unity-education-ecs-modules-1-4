using Unity.Entities;
using UnityEngine;

public abstract class CollisionAction : MonoBehaviour
{
    public abstract void Execute(Entity targetEntity, EntityManager entityManager, EntityCommandBuffer commandBuffer);
}
