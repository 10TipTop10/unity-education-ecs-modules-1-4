using Unity.Entities;
using Unity.Mathematics;

public struct MoveRequestComponent : IComponentData
{
    public float3 RequestedOffset;
}
