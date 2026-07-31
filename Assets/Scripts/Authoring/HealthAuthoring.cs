using UnityEngine;

public class HealthAuthoring : MonoBehaviour
{
    public Settings settings;
    [HideInInspector] public int MaxHealth;

    private void Start()
    {
        MaxHealth = settings.HeroHealth;
    }
}
