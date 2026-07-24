using Unity.Entities;
using Unity.Mathematics;
public struct UserInputComponent : IComponentData
{
    public float2 InputDirection;
    public float ShootInput;
    public float DashInput;
}
