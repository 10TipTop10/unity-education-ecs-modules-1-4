using Unity.Entities;

public struct BulletComponent : IComponentData
{
    public float BulletSpeed;
    public float BulletLifetime;
    public float CollisionRadius;
    public int CollisionMask;
}