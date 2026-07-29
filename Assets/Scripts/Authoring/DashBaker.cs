using Unity.Entities;

public class DashBaker : Baker<DashAuthoring>
{
    public override void Bake(DashAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new DashComponent
        {
            DashDistance = authoring.DashDistance,
            DashDelay = authoring.DashDelay,
        });
    }
}
