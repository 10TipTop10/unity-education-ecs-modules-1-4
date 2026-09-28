using UnityEngine;

[RequireComponent(typeof(Durability))]
public class ScrapReward : MonoBehaviour
{
    [SerializeField] private int _scrapAmount = 5;

    private Durability _durability;
    private ScrapStorage _storage;

    private void Awake()
    {
        _durability = GetComponent<Durability>();
    }

    public void SetStorage(ScrapStorage storage)
    {
        _storage = storage;
    }

    private void OnEnable()
    {
        _durability.Depleted += OnDepleted;
    }

    private void OnDepleted()
    {
        if(_storage == null) return;

        _storage.AddScrap(_scrapAmount);

        Debug.Log($"Сырьё: {_storage.CurrentScrap}");
    }

    private void OnDisable()
    {
        _durability.Depleted -= OnDepleted;
    }
}
