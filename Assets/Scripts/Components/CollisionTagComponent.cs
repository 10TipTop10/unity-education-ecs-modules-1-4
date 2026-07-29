using Unity.Collections;
using Unity.Entities;

public struct CollisionTagComponent : IComponentData
{
    public FixedString64Bytes CollisionTag;
}
