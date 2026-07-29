using Unity.Entities;

public class HealthBaker : Baker<HealthAuthoring>
{
    public override void Bake(HealthAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new HealthComponent
        {
            MaxHealth = authoring.MaxHealth,
            CurrentHealth = authoring.MaxHealth
        });
    }
}
