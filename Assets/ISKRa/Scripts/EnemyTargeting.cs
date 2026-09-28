using UnityEngine;

public class EnemyTargeting : MonoBehaviour
{
    [SerializeField] private Transform _rotatingPart;
    [SerializeField] private Transform _verticalRotatingPart;
    [SerializeField] private float _targetRange = 6f;
    [SerializeField] private float _rotatingSpeed = 360f;
    [SerializeField] private float _verticalRotatingSpeed = 180f;
    [SerializeField] private float _minElevation = -10f;
    [SerializeField] private float _maxElevation = 60f;
    [SerializeField] private float _returnDelay = 2f;

    private Quaternion _startHorizontalRotation;
    private Quaternion _startVerticalRotation;
    private float _timeWithoutTarget;
    private readonly Collider[] _results = new Collider[64];

    public EnemyTarget CurrentTarget { get; private set; }

    private void Awake()
    {
        _startHorizontalRotation = _rotatingPart.localRotation;

        if (_verticalRotatingPart != null)
        {
            _startVerticalRotation = _verticalRotatingPart.localRotation;
        }
    }

    private void Update()
    {
        CurrentTarget = FindNearestTarget();

        if (CurrentTarget != null)
        {
            _timeWithoutTarget = 0f;

            Vector3 targetPosition = CurrentTarget.AimPosition;

            RotateHorizontal(targetPosition);
            RotateVertical(targetPosition);

            return;
        }

        _timeWithoutTarget += Time.deltaTime;

        if(_timeWithoutTarget >= _returnDelay)
        {
            ReturnToStartRotation();
        }

    }

    private void RotateVertical(Vector3 targetPosition)
    {
        if (_verticalRotatingPart == null) return;

        Vector3 worldDirection = targetPosition - _verticalRotatingPart.position;
        Vector3 localDirection = _verticalRotatingPart.parent.InverseTransformDirection(worldDirection);

        if (localDirection.sqrMagnitude < 0.001f) return;

        float elevation = Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg;

        elevation = Mathf.Clamp(elevation, _minElevation, _maxElevation);

        Quaternion targetRotation = Quaternion.Euler(-elevation, 0f, 0f);

        _verticalRotatingPart.localRotation = Quaternion.RotateTowards(_verticalRotatingPart.localRotation, targetRotation, _verticalRotatingSpeed * Time.deltaTime);
    }

    private void RotateHorizontal(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - _rotatingPart.position;
        direction.y = 0;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        _rotatingPart.rotation = Quaternion.RotateTowards(_rotatingPart.rotation, targetRotation, _rotatingSpeed * Time.deltaTime);
    }

    private void ReturnToStartRotation()
    {
        Quaternion horizontalRotation = Quaternion.RotateTowards(_rotatingPart.localRotation, _startHorizontalRotation, _rotatingSpeed * Time.deltaTime);
        _rotatingPart.localRotation = horizontalRotation;

        if (_verticalRotatingPart == null) return;

        Quaternion verticalRotation = Quaternion.RotateTowards(_verticalRotatingPart.localRotation, _startVerticalRotation, _verticalRotatingSpeed * Time.deltaTime);
        _verticalRotatingPart.localRotation = verticalRotation;
    }

    private EnemyTarget FindNearestTarget()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, _targetRange, _results, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        EnemyTarget nearest = null;

        float nearestDistanceSquared = _targetRange * _targetRange;

        for (int i = 0; i < count; i++)
        {
            EnemyTarget target = _results[i].GetComponentInParent<EnemyTarget>();

            if (target == null) continue;

            if (target.Durability == null || target.Durability.CurrentDurability <= 0) continue;

            float distanceSquared = (target.AimPosition - transform.position).sqrMagnitude;

            if (distanceSquared >= nearestDistanceSquared) continue;

            nearestDistanceSquared = distanceSquared;
            nearest = target;
        }

        return nearest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _targetRange);
    }
}