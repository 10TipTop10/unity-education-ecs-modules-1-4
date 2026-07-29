using Unity.Entities;

public class DestroyGameObjectAction : CollisionAction
{
    public override void Execute(Entity targetEntity, EntityManager entityManager, EntityCommandBuffer commandBuffer)
    {
        Destroy(gameObject);
    }
}
