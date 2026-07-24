using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class CharacterMoveSystem : SystemBase
{
    protected override void OnCreate()
    {

    }
    protected override void OnUpdate()
    {
        foreach (var (transform, move, input) in SystemAPI.Query<
            RefRW<LocalTransform>,
            RefRO<MoveComponent>,
            RefRO<UserInputComponent>
            >())
        {
            float3 pos = transform.ValueRW.Position;
            float3 direction = new float3(input.ValueRO.InputDirection.x, 0, input.ValueRO.InputDirection.y);
            pos += direction * move.ValueRO._moveSpeed * SystemAPI.Time.DeltaTime;
            transform.ValueRW.Position = pos;
            if (math.lengthsq(direction) != 0)
            {
                transform.ValueRW.Rotation = quaternion.LookRotationSafe(direction, new float3(0, 1, 0));
            }
        }
    }
}
