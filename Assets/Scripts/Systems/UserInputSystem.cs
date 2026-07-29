using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.InputSystem;


public partial class UserInputSystem : SystemBase
{
    private InputAction _moveAction;
    private InputAction _shootAction;
    private InputAction _dashAction;
    private float2 _moveInput;
    private float _shootInput;
    private float _dashInput;


    protected override void OnCreate()
    {
        _moveAction = new InputAction("move", binding: "<Gamepad>/rightStick");
        _moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/W")
            .With("Down", "<Keyboard>/S")
            .With("Left", "<Keyboard>/A")
            .With("Right", "<Keyboard>/D");
        _moveAction.performed += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.started += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.canceled += context => { _moveInput = context.ReadValue<Vector2>(); };

        _shootAction = new InputAction("shoot", binding: "<Keyboard>/Space");
        _shootAction.performed += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.started += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.canceled += context => { _shootInput = context.ReadValue<float>(); };

        _dashAction = new InputAction("dash", binding: "<Keyboard>/leftShift");
        _dashAction.performed += context => { _dashInput = context.ReadValue<float>(); };
        _dashAction.started += context => { _dashInput = context.ReadValue<float>(); };
        _dashAction.canceled += context => { _dashInput = context.ReadValue<float>(); };

        RequireForUpdate<UserInputComponent>();
    }

    protected override void OnStartRunning()
    {
        _moveAction.Enable();
        _shootAction.Enable();
        _dashAction.Enable();
    }

    protected override void OnStopRunning()
    {
        _moveAction.Disable();
        _shootAction.Disable();
        _dashAction.Disable();
    }

    protected override void OnUpdate()
    {
        foreach (RefRW<UserInputComponent> inputComponent in SystemAPI.Query<RefRW<UserInputComponent>>())
        {
            inputComponent.ValueRW.InputDirection = _moveInput;
            inputComponent.ValueRW.ShootInput = _shootInput;
            inputComponent.ValueRW.DashInput = _dashInput;
        }
    }

    protected override void OnDestroy()
    {
        _moveAction.Dispose();
        _shootAction.Dispose();
        _dashAction.Dispose();
    }
}
