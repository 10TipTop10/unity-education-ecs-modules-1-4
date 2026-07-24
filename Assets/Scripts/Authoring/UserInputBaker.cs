using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class UserInputBaker : Baker <UserInputAuthoring>
{
    public override void Bake(UserInputAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<UserInputComponent>(entity);
    }
}
