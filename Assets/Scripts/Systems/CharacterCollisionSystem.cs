using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

[UpdateAfter(typeof(DashSystem))]
public partial class CharacterCollisionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        foreach (var (transform, move, moveRequest, collision) in SystemAPI.Query<
            RefRW<LocalTransform>,
            RefRO<MoveComponent>,
            RefRW<MoveRequestComponent>,
            RefRO<CollisionComponent>
            >())
        {
            float3 requestedOffset = moveRequest.ValueRO.RequestedOffset;
            float moveDistance = math.length(requestedOffset);
            if (moveDistance == 0f) continue;

            float3 moveDirection = requestedOffset / moveDistance;

            if (collision.ValueRO.ShapeType != CollisionShapeType.Capsule) continue;

            float3 worldPointA = transform.ValueRO.TransformPoint(collision.ValueRO.LocalPointA);
            float3 worldPointB = transform.ValueRO.TransformPoint(collision.ValueRO.LocalPointB);

            float worldRadius = collision.ValueRO.Radius * math.abs(transform.ValueRO.Scale);

            bool isBlocked = Physics.CapsuleCast(worldPointA, worldPointB, worldRadius, moveDirection, out RaycastHit hit, moveDistance, move.ValueRO.BlockingMask, QueryTriggerInteraction.Ignore);

            if (isBlocked)
            {
                float allowedDistance = math.max(0f, hit.distance - 0.01f);
                transform.ValueRW.Position += moveDirection * allowedDistance;
            }
            else
            {
                transform.ValueRW.Position += requestedOffset;
            }
            
            moveRequest.ValueRW.RequestedOffset = float3.zero;

        }
    }
}
