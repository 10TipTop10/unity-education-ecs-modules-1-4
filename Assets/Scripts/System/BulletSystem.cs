using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;

public partial class BulletSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach(var (transform, bullet, entity) in SystemAPI.Query<
            RefRW<LocalTransform>,
            RefRW<BulletComponent>
            >().WithEntityAccess())
        {
            float3 direction = transform.ValueRO.Forward();
            transform.ValueRW.Position += direction * bullet.ValueRO.bulletSpeed * SystemAPI.Time.DeltaTime;
            bullet.ValueRW.bulletLifetime-=SystemAPI.Time.DeltaTime;
            
            if(bullet.ValueRO.bulletLifetime <= 0)
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
