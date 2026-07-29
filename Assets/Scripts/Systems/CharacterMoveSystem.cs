using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class CharacterMoveSystem : SystemBase
{

    protected override void OnUpdate()
    {
        foreach (var (transform, move, input, requestedOffset) in SystemAPI.Query<
            RefRW<LocalTransform>,
            RefRO<MoveComponent>,
            RefRO<UserInputComponent>,
            RefRW<MoveRequestComponent>
            >())
        {
            float3 direction = new float3(input.ValueRO.InputDirection.x, 0, input.ValueRO.InputDirection.y);
            requestedOffset.ValueRW.RequestedOffset = direction * move.ValueRO.MoveSpeed * SystemAPI.Time.DeltaTime;

            if (math.lengthsq(direction) != 0)
            {
                transform.ValueRW.Rotation = quaternion.LookRotationSafe(direction, new float3(0, 1, 0));
            }
        }
    }
}
