using Unity.Entities;
using Unity.Mathematics;

public enum CollisionShapeType
{
    Sphere,
    Capsule,
    Box
}

public struct CollisionComponent : IComponentData
{
    public CollisionShapeType ShapeType;
    public float3 LocalPointA;
    public float3 LocalPointB;
    public float3 LocalCenter;
    public float3 HalfExtents;
    public float Radius;
}
