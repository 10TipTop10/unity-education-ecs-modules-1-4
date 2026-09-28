using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputReader _input;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _transform;
    [SerializeField] private Camera _camera;

    [SerializeField] private float _forwardSpeed = 4f;
    [SerializeField] private float _reverseSpeed = 2.5f;
    [SerializeField] private float _rotationSpeed = 240f;

    [SerializeField] private float _enterReverseThreshold = -0.65f;
    [SerializeField] private float _exitReverseThreshold = -0.5f;

    private bool _isReversing;

    private void Update()
    {
        Vector2 input = _input.MoveDirection;

        if (input.sqrMagnitude < 0.01f) return;

        float inputMagnitude = Mathf.Clamp01(input.magnitude);


        Vector3 cameraForward = _camera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = _camera.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 desiredDirection = (cameraRight * input.x + cameraForward * input.y).normalized;

        float alignment = Vector3.Dot(_transform.forward, desiredDirection);

        UpdateDrivingDirection(alignment);

        Vector3 desiredForward = _isReversing ? -desiredDirection : desiredDirection;

        Quaternion targetRotation = Quaternion.LookRotation(desiredForward, Vector3.up);

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        Move(desiredDirection, inputMagnitude);
    }

    private void Move(Vector3 desiredDirection, float inputMagnitude)
    {
        float alignment = Vector3.Dot(_transform.forward, desiredDirection);

        float facingAmount = _isReversing ? Mathf.Clamp01(-alignment) : Mathf.Clamp01(alignment);

        float speed = _isReversing ? _reverseSpeed : _forwardSpeed;

        float directionSign = _isReversing ? -1f : 1f;

        Vector3 velocity = _transform.forward * directionSign * speed * facingAmount * inputMagnitude;

        _controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateDrivingDirection(float alignment)
    {
        if (_isReversing)
        {
            if (alignment > _exitReverseThreshold)
            {
                _isReversing = false;
            }
        }
        else
        {
            if (alignment < _enterReverseThreshold)
            {
                _isReversing = true;
            }
        }
    }
}
