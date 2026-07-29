using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public partial class CharacterShootSystem : SystemBase
{
    protected override void OnCreate()
    {

    }
    protected override void OnUpdate()
    {
        double currentTime = SystemAPI.Time.ElapsedTime;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (shoot, input, transform, entity) in SystemAPI.Query<
            RefRW<ShootComponent>,
            RefRO<UserInputComponent>,
            RefRO<LocalTransform>
            >().WithEntityAccess())
        {
            if (input.ValueRO.ShootInput <= 0f) continue;
            if (currentTime <= shoot.ValueRO.NextShootTime) continue;

            Entity projectile = ecb.Instantiate(shoot.ValueRO.ProjectilePrefab);
            if (SystemAPI.HasComponent<RicochetPerkComponent>(entity))
            {
                ecb.AddComponent<RicochetProjectileComponent>(projectile);
            }
            ecb.SetComponent(projectile, LocalTransform.FromPositionRotation(transform.ValueRO.Position, transform.ValueRO.Rotation));
            shoot.ValueRW.NextShootTime = currentTime + shoot.ValueRO.ShootDelay;

        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
