using UnityEngine;

public class LaserWeapon : Weapon
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _fireInterval = 0.4f;
    [SerializeField] private LineRenderer _beamRenderer;
    [SerializeField] private float _beamDuration = 0.12f;

    private float _beamHideTime;
    private float _nextFireTime;

    private void Update()
    {
        if (_beamRenderer != null && _beamRenderer.enabled && Time.time >= _beamHideTime)
        {
            _beamRenderer.enabled = false;
        }
    }

    public override void Tick(EnemyTarget target)
    {
        if (!CanAttack(target))
            return;

        if (Time.time < _nextFireTime)
            return;

        Vector3 startBeamPos = _firePoint.position;
        Vector3 endBeamPos = target.AimPosition;

        target.Durability.TakeDamage(_damage);

        if(_beamRenderer != null)
        {
            _beamRenderer.SetPosition(0, startBeamPos);
            _beamRenderer.SetPosition(1, endBeamPos);
            _beamRenderer.enabled = true;
            _beamHideTime = Time.time + _beamDuration;
        }
        
        _nextFireTime = Time.time + _fireInterval;
    }
}
