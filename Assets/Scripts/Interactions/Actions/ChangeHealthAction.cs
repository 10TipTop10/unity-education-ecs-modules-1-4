using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ChangeHealthAction : CollisionAction
{
    [SerializeField] private HealingEffectManager _healEffectManager;
    [SerializeField] private int _amount;
    public override void Execute(Entity targetEntity, EntityManager entityManager, EntityCommandBuffer commandBuffer)
    {
        if (!entityManager.HasComponent<HealthComponent>(targetEntity)) return;

        HealthComponent health = entityManager.GetComponentData<HealthComponent>(targetEntity);

        health.CurrentHealth = math.clamp(health.CurrentHealth + _amount, 0, health.MaxHealth);

        entityManager.SetComponentData(targetEntity, health);

        if (_amount > 0)
        {
            _healEffectManager.PlayHealingEffect();
        }
    }
}
