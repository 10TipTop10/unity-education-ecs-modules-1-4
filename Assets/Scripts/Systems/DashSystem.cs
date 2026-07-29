using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(CharacterMoveSystem))]
public partial class DashSystem : SystemBase
{
    protected override void OnUpdate()
    {
        double currentTime = SystemAPI.Time.ElapsedTime;
        foreach (var (transform, dash, input, moveRequest) in SystemAPI.Query<
            RefRW<LocalTransform>,
            RefRW<DashComponent>,
            RefRO<UserInputComponent>,
            RefRW<MoveRequestComponent>
                >())
        {
            if (input.ValueRO.DashInput <= 0) continue;
            if (currentTime <= dash.ValueRO.NextDashTime) continue;

            float3 direction = transform.ValueRO.Forward();
            float3 dashOffset = direction * dash.ValueRO.DashDistance;
            moveRequest.ValueRW.RequestedOffset += dashOffset;
            dash.ValueRW.NextDashTime = currentTime + dash.ValueRO.DashDelay;
        }
    }
}
