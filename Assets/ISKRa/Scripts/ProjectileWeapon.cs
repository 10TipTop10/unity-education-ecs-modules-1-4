using System;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField] private Missile _missilePrefab;
    [SerializeField] private int _damage = 25;
    [SerializeField] private int _missilesPerBurst = 6;
    [SerializeField] private float _shotInterval = 0.15f;
    [SerializeField] private float _reloadDuration = 4f;

    public event Action AmmoStateChanged;

    private int _missilesRemaining;
    private float _nextShotTime;
    private float _reloadCompleteTime;
    private bool _isReloading;

    public int MissilesRemaining => _missilesRemaining;
    public int MagazineSize => _missilesPerBurst;
    public bool IsReloading => _isReloading;


    private void Awake()
    {
        _missilesRemaining = _missilesPerBurst;
    }

    public override void Tick(EnemyTarget target)
    {
        UpdateReload();

        if (_isReloading)
            return;

        if (!CanAttack(target))
            return;

        if (Time.time < _nextShotTime)
            return;

        LaunchMissile(target);
    }

    private void UpdateReload()
    {
        if (!_isReloading)
            return;

        if (Time.time < _reloadCompleteTime)
            return;

        _missilesRemaining = _missilesPerBurst;
        _isReloading = false;
        AmmoStateChanged?.Invoke();
    }

    private void LaunchMissile(EnemyTarget target)
    {
        if (_missilePrefab == null)
            return;

        Missile missile = Instantiate(
            _missilePrefab,
            _firePoint.position,
            _firePoint.rotation);

        missile.Initialize(target, _damage);

        _missilesRemaining--;

        if (_missilesRemaining <= 0)
        {
            _isReloading = true;
            _reloadCompleteTime = Time.time + _reloadDuration;
        }
        else
        {
            _nextShotTime = Time.time + _shotInterval;
        }

        AmmoStateChanged?.Invoke();
    }
}
