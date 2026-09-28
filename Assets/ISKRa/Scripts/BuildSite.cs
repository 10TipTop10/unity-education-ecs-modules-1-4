using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildSite : MonoBehaviour
{
    [SerializeField] private GameObject _buildingPrefab;
    [SerializeField] private Transform _buildPoint;
    [SerializeField] private GameObject _siteVisual;
    [SerializeField] private Collider _buildZone;

    [SerializeField] private ScrapStorage _storage;
    [SerializeField] private int _buildCost = 40;
    [SerializeField] private float _paymentInterval = 0.1f;

    private int _paidScrap;
    private float _nextPaymentTime;
    private bool _isCompleted;

    public event Action<int, int> ProgressChanged;
    public event Action Completed;
    public int PaidScrap => _paidScrap;
    public int BuildCost => _buildCost;
    public bool IsCompleted => _isCompleted;

    private readonly HashSet<Collider> _playerColliders = new();

    public bool PlayerInside => _playerColliders.Count > 0;

    private void Update()
    {
        if (!PlayerInside || _isCompleted) return;
        if (Time.time < _nextPaymentTime) return;

        _nextPaymentTime = Time.time + _paymentInterval;

        if (!_storage.TrySpendScrap(1)) return;

        _paidScrap++;

        ProgressChanged?.Invoke(_paidScrap, _buildCost);

        if(_paidScrap >= _buildCost)
        {
            CompleteBuild();
        }
    }

    private void CompleteBuild()
    {
        _isCompleted = true;

        if (_buildingPrefab == null) return;

        Instantiate(_buildingPrefab, _buildPoint.position, _buildPoint.rotation);

        if(_siteVisual != null)
        {
            _siteVisual.SetActive(false);
        }
        if(_buildZone!= null)
        {
            _buildZone.enabled = false;
        }

        _playerColliders.Clear();
        Completed?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerIdentity>() == null) return;

        bool playerWasOutside = !PlayerInside;

        _playerColliders.Add(other);

        if (playerWasOutside && PlayerInside)
        {
            //Debug.Log("ИСКРА вошла в зону строительства");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_playerColliders.Remove(other)) return;

        if (!PlayerInside)
        {
            //Debug.Log("ИСКРА покинула зону строительства");
        }
    }

    private void OnDisable()
    {
        _playerColliders.Clear();
    }
}
