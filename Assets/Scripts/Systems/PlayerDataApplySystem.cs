using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class PlayerDataApplySystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingleton<LoadedPlayerDataComponent>(out LoadedPlayerDataComponent loadedData)) return;

        Entity updateEntity = SystemAPI.GetSingletonEntity<LoadedPlayerDataComponent>();

        bool dataApplied = false;

        foreach (var (health, move, dash) in SystemAPI.Query<
            RefRW<HealthComponent>,
            RefRW<MoveComponent>,
            RefRW<DashComponent>
            >().WithAll<UserInputComponent>())
        {
            health.ValueRW.CurrentHealth = (int)math.round(CalculateScaledValue(health.ValueRO.CurrentHealth, health.ValueRO.MaxHealth, loadedData.MaxHealth));
            health.ValueRW.MaxHealth = loadedData.MaxHealth;

            move.ValueRW.MoveSpeed = loadedData.MoveSpeed;
            dash.ValueRW.DashDistance = loadedData.DashDistance;

            dataApplied = true;
        }

        if (dataApplied)
        {
            EntityManager.DestroyEntity(updateEntity);
        }
    }

    private static float CalculateScaledValue(int oldCurrentValue, int oldMaxValue, int newMaxValue)
    {
        if (oldMaxValue <= 0) return newMaxValue;
        float numberCoefficient = (float)oldCurrentValue / oldMaxValue;
        float newCurrentValue = newMaxValue * numberCoefficient;
        return newCurrentValue;
    }
}
