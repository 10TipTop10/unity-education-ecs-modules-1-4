using Unity.Entities;

public struct MoveComponent : IComponentData
{
    public float MoveSpeed;
    public int BlockingMask;
}
