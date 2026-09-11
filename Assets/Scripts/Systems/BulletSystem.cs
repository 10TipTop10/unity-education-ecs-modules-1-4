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
        foreach (var (transform, bullet, entity) in SystemAPI.Query<
            RefRW<LocalTransform>,
            RefRW<BulletComponent>
            >().WithEntityAccess())
        {
            float3 direction = transform.ValueRO.Forward();
            float travelDistance = bullet.ValueRO.BulletSpeed * SystemAPI.Time.DeltaTime;
            float worldRadius = bullet.ValueRO.CollisionRadius * transform.ValueRO.Scale;

            bool boundaryHit = Physics.SphereCast(transform.ValueRO.Position, worldRadius, direction, out RaycastHit hit, travelDistance, bullet.ValueRO.CollisionMask, QueryTriggerInteraction.Ignore);

            if (boundaryHit)
            {
                if (SystemAPI.HasComponent<RicochetProjectileComponent>(entity))
                {
                    float3 reflectedDirection = math.reflect(direction, hit.normal);
                    float3 impactCenter = transform.ValueRO.Position + direction * hit.distance;

                    if (BulletImpactManager.Instance != null)
                    {
                        BulletImpactManager.Instance.InstantiateRicochetEffect(hit.point, hit.normal);
                    }

                    transform.ValueRW.Rotation = quaternion.LookRotationSafe(reflectedDirection, math.up());
                    transform.ValueRW.Position = impactCenter + (float3)hit.normal * 0.001f;
                }
                else
                {
                    if(BulletImpactManager.Instance != null)
                    {
                        BulletImpactManager.Instance.InstantiateHitEffect(hit.point, hit.normal);
                    }
                    ecb.DestroyEntity(entity);
                    continue;
                }
            }
            else
            {
                transform.ValueRW.Position += direction * travelDistance;
            }

            bullet.ValueRW.BulletLifetime -= SystemAPI.Time.DeltaTime;

            if (bullet.ValueRO.BulletLifetime <= 0)
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
