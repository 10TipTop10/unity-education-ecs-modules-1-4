using Unity.Entities;
public class BulletBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new BulletComponent
        {
            bulletSpeed = authoring._bulletSpeed,
            bulletLifetime = authoring._bulletLifetime
        });
    }
}