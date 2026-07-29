using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class CharacterBoundsSystem : SystemBase
{
    private const float BoundsDistance = 10f;

    protected override void OnUpdate()
    {
        foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<UserInputComponent>())
        {
            float3 pos = transform.ValueRO.Position;
            pos.x = math.clamp(pos.x, -BoundsDistance, BoundsDistance);
            pos.z = math.clamp(pos.z, -BoundsDistance, BoundsDistance);
            transform.ValueRW.Position = pos;
        }
    }
}
