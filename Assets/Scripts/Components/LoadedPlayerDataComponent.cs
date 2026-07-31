using Unity.Entities;

public struct LoadedPlayerDataComponent : IComponentData
{
    public int MaxHealth;
    public float MoveSpeed;
    public float DashDistance;
}
