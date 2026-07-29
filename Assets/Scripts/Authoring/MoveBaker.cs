using Unity.Entities;

public class MoveBaker : Baker<MoveAuthoring>
{
    public override void Bake(MoveAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new MoveComponent
        {
            MoveSpeed = authoring.MoveSpeed,
            BlockingMask = authoring.BlockingMask.value
        });
        AddComponent(entity, new MoveRequestComponent());
        
    }
}
