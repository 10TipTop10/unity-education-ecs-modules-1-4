using Unity.Entities;

public struct DashComponent : IComponentData
{
    public float DashDistance;
    public float DashDelay;
    public double NextDashTime;
}
