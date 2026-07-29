using Unity.Entities;

public struct ShootComponent : IComponentData
{
    public Entity ProjectilePrefab;
    public float ShootDelay;
    public double NextShootTime;
}
