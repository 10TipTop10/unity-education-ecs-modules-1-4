using Unity.Entities;

public class ShootBaker : Baker<ShootAuthoring>
{
    public override void Bake(ShootAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        Entity projectileEntity = GetEntity(authoring._bulletPrefab, TransformUsageFlags.Dynamic);

        AddComponent(entity, new ShootComponent
        {
            ProjectilePrefab = projectileEntity,
            ShootDelay = authoring._shootDelay
        });
    }
}
