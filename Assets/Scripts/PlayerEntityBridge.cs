using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerEntityBridge : MonoBehaviour
{
    private EntityManager _entityManager;
    private EntityQuery _entityQuery;

    private void Start()
    {
        var entityWorld = World.DefaultGameObjectInjectionWorld;
        _entityManager = entityWorld.EntityManager;

        _entityQuery = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<UserInputComponent>(), ComponentType.ReadOnly<LocalTransform>());
    }

    public bool TryTakeDamage(int damage)
    {
        if (!TryGetPlayerEntity(out Entity playerEntity)) return false;
        if (!_entityManager.HasComponent<HealthComponent>(playerEntity)) return false;

        HealthComponent health = _entityManager.GetComponentData<HealthComponent>(playerEntity);

        health.CurrentHealth = math.clamp(health.CurrentHealth - damage, 0, health.MaxHealth);

        _entityManager.SetComponentData(playerEntity, health);

        Debug.Log($"Health: {health.CurrentHealth}");

        return true;
    }

    public bool TryGetPlayerPosition(out Vector3 playerPosition)
    {
        playerPosition = Vector3.zero;
        if (TryGetPlayerEntity(out var playerEntity))
        {
            playerPosition = _entityManager.GetComponentData<LocalTransform>(playerEntity).Position;
            return true;
        }
        return false;
    }

    public bool TryGetPlayerEntity(out Entity playerEntity)
    {
        if (!_entityQuery.TryGetSingletonEntity<UserInputComponent>(out playerEntity))
        {
            return false;
        }

        return true;
    }
}