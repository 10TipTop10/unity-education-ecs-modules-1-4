using UnityEngine;

public class BulletImpactManager : MonoBehaviour
{
    [SerializeField] private GameObject _hitEffectPrefab;
    [SerializeField] private GameObject _ricochetEffectPrefab;

    public static BulletImpactManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void InstantiateRicochetEffect(Vector3 position, Vector3 normal)
    {
        Instantiate(_ricochetEffectPrefab, position, Quaternion.LookRotation(normal));
    }

    public void InstantiateHitEffect(Vector3 position, Vector3 normal)
    {
        Instantiate(_hitEffectPrefab, position, Quaternion.LookRotation(normal));
    }

    private void OnDestroy()
    {
        if (Instance != this) return;
        Instance = null;
    }
}
