using Unity.Entities;

public class DashBaker : Baker<DashAuthoring>
{
    public override void Bake(DashAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new DashComponent
        {
            dashDistance = authoring._dashDistance,
            dashDelay = authoring._dashDelay,
        });
    }
}
