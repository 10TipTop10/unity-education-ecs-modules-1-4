using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class DashSystem : SystemBase
{
    protected override void OnUpdate()
    {
        double currentTime = SystemAPI.Time.ElapsedTime;
        foreach (var (transform, dash, input) in SystemAPI.Query<
            RefRW<LocalTransform>,
            RefRW<DashComponent>,
            RefRO<UserInputComponent>>())
        {
            if (input.ValueRO.DashInput <= 0) continue;
            if (currentTime <= dash.ValueRO.NextDashTime) continue;

            float3 direction = transform.ValueRO.Forward();
            float3 dashOffset = direction * dash.ValueRO.dashDistance;
            transform.ValueRW.Position += dashOffset;

            dash.ValueRW.NextDashTime = currentTime + dash.ValueRO.dashDelay;
        }
    }
}
