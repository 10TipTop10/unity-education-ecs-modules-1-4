using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;

public class CollisionBaker : Baker<CollisionAuthoring>
{
    public override void Bake(CollisionAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        var collider = authoring.SourceCollider;

        switch (collider)
        {
            case SphereCollider sphere:
                {
                    AddComponent(entity, new CollisionComponent
                    {
                        ShapeType = CollisionShapeType.Sphere,
                        Radius = sphere.radius,
                        LocalCenter = sphere.center,
                    });
                    break;
                }
            case CapsuleCollider capsule:
                {
                    float radius = capsule.radius;
                    float halfSegment = math.max(0f, capsule.height / 2 - radius);
                    float3 colliderCenter = capsule.center;
                    float3 directionAxis;

                    switch (capsule.direction)
                    {
                        case 0:
                            directionAxis = new float3(1, 0, 0);
                            break;
                        case 1:
                            directionAxis = new float3(0, 1, 0);
                            break;
                        case 2:
                            directionAxis = new float3(0, 0, 1);
                            break;
                        default:
                            throw new System.ArgumentOutOfRangeException();
                    }

                    AddComponent(entity, new CollisionComponent
                    {
                        ShapeType = CollisionShapeType.Capsule,
                        Radius = radius,
                        LocalPointA = colliderCenter + directionAxis * halfSegment,
                        LocalPointB = colliderCenter - directionAxis * halfSegment,
                    });

                    break;
                }
            case BoxCollider box:
                {
                    float3 halfExtents = box.size * 0.5f;
                    AddComponent(entity, new CollisionComponent
                    {
                        ShapeType = CollisionShapeType.Box,
                        LocalCenter = box.center,
                        HalfExtents = halfExtents
                    });
                    break;
                }
            default:
                {
                    throw new System.ArgumentOutOfRangeException();
                } 
        }

        AddComponent(entity, new CollisionTagComponent
        {
            CollisionTag = new FixedString64Bytes(authoring.gameObject.tag)
        });
    }
}
