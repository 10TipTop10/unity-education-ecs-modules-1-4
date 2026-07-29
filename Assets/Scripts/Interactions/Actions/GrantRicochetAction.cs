using Unity.Entities;

public class GrantRicochetAction : CollisionAction
{
    public override void Execute(Entity targetEntity, EntityManager entityManager, EntityCommandBuffer commandBuffer)
    {
        if (entityManager.HasComponent<RicochetPerkComponent>(targetEntity)) return;

        commandBuffer.AddComponent<RicochetPerkComponent>(targetEntity);
    }
}
