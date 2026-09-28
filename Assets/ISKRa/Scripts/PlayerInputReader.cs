using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private InputAction _moveAction;
    public Vector2 MoveDirection {  get; private set; }

    private void Awake()
    {
        _moveAction = new InputAction(name: "Move", type: InputActionType.Value);
        
        _moveAction.AddBinding("<Gamepad>/rightStick");

        _moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
    }

    private void OnEnable()
    {
        _moveAction.Enable();
    }

    private void Update()
    {
        MoveDirection = _moveAction.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
    }

    private void OnDestroy()
    {
        _moveAction.Dispose();
    }
}
