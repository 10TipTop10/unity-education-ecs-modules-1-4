using Unity.Entities;
public class BulletBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new BulletComponent
        {
            BulletSpeed = authoring.BulletSpeed,
            BulletLifetime = authoring.BulletLifetime,
            CollisionRadius = authoring.CollisionRadius,
            CollisionMask = authoring.CollisionMask.value
        });
    }
}