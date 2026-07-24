using Unity.Entities;

public struct DashComponent : IComponentData
{
    public float dashDistance;
    public float dashDelay;
    public double NextDashTime;
}
