using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private Durability _durability;

    private void OnEnable()
    {
        _durability.Depleted += OnDepleted;
    }
    private void OnDepleted()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        _durability.Depleted -= OnDepleted;
    }
}
