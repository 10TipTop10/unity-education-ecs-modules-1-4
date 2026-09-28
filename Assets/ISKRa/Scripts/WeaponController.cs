using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private EnemyTargeting _targeting;
    [SerializeField] private Weapon[] _weapons;

    private void Update()
    {
        EnemyTarget target = _targeting.CurrentTarget;

        foreach (Weapon weapon in _weapons)
        {
            if (weapon != null)
                weapon.Tick(target);
        }
    }
}
