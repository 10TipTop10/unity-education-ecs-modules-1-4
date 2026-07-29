using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;

public partial class CollisionSystem : SystemBase
{
    private readonly Collider[] _results = new Collider[50];
    private readonly Dictionary<Entity, HashSet<CollisionAbility>> _previousAbilitiesByEntity = new();
    private readonly HashSet<CollisionAbility> _currentAbilities = new();

    protected override void OnUpdate()
    {
        var commandBuffer = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (transform, collision, collisionTag, entity) in SystemAPI.Query<
            RefRO<LocalTransform>,
            RefRO<CollisionComponent>,
            RefRO<CollisionTagComponent>
            >().WithEntityAccess())
        {
            _currentAbilities.Clear();
            if (!_previousAbilitiesByEntity.TryGetValue(entity, out var previousAbilities))
            {
                previousAbilities = new HashSet<CollisionAbility>();
                _previousAbilitiesByEntity.Add(entity, previousAbilities);
            }

            int collisionCount;

            switch (collision.ValueRO.ShapeType)
            {
                case CollisionShapeType.Sphere:
                    {

                        var worldCenter = transform.ValueRO.TransformPoint(collision.ValueRO.LocalCenter);
                        var worldRadius = collision.ValueRO.Radius * math.abs(transform.ValueRO.Scale);

                        collisionCount = Physics.OverlapSphereNonAlloc(worldCenter, worldRadius, _results);

                        break;
                    }
                case CollisionShapeType.Capsule:
                    {
                        var worldPointA = transform.ValueRO.TransformPoint(collision.ValueRO.LocalPointA);
                        var worldPointB = transform.ValueRO.TransformPoint(collision.ValueRO.LocalPointB);
                        var worldRadius = collision.ValueRO.Radius * math.abs(transform.ValueRO.Scale);

                        collisionCount = Physics.OverlapCapsuleNonAlloc(worldPointA, worldPointB, worldRadius, _results);

                        break;
                    }
                case CollisionShapeType.Box:
                    {
                        var worldCenter = transform.ValueRO.TransformPoint(collision.ValueRO.LocalCenter);
                        var worldHalfExtents = collision.ValueRO.HalfExtents * math.abs(transform.ValueRO.Scale);
                        var worldRotation = transform.ValueRO.Rotation;

                        collisionCount = Physics.OverlapBoxNonAlloc(worldCenter, worldHalfExtents, _results, worldRotation);

                        break;
                    }
                default:
                    throw new System.ArgumentOutOfRangeException();
            }

            for (int i = 0; i < collisionCount; i++)
            {

                var currentCollisionAbility = _results[i].GetComponent<CollisionAbility>();

                if (currentCollisionAbility == null) continue;

                var requiredTag = new FixedString64Bytes(currentCollisionAbility.TargetTag);

                if (requiredTag != collisionTag.ValueRO.CollisionTag) continue;

                bool isFirstOccurenceThisFrame = _currentAbilities.Add(currentCollisionAbility);

                if (!isFirstOccurenceThisFrame) continue;

                if (previousAbilities.Contains(currentCollisionAbility)) continue;

                currentCollisionAbility.ExecuteActions(entity, EntityManager, commandBuffer);
            }
            previousAbilities.Clear();
            previousAbilities.UnionWith(_currentAbilities);
        }
        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();
    }
}
