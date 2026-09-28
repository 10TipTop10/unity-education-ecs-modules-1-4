using System.Collections;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private Durability _durability;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _playerRespawnPoint;
    [SerializeField] private GameObject _visualPlayer;
    [SerializeField] private Behaviour[] _gameplayComponents;
    [SerializeField] private float _respawnDelay = 3f;

    private bool _isDestroyed;

    private void OnEnable()
    {
        _durability.Depleted += OnDepleted;  
    }

    private void OnDepleted()
    {
        if (_isDestroyed) return;
        StartCoroutine(RecoveryCoroutine());
    }

    private void OnDisable()
    {
        _durability.Depleted -= OnDepleted;
    }

    private IEnumerator RecoveryCoroutine()
    {
        _isDestroyed = true;
        _characterController.enabled = false;
        SetGameplayEnabled(false);
        _visualPlayer.SetActive(false);

        transform.SetPositionAndRotation(_playerRespawnPoint.position, _playerRespawnPoint.rotation);

        yield return new WaitForSeconds(_respawnDelay);

        _durability.RestoreFull();
        _visualPlayer.SetActive(true);
        SetGameplayEnabled(true);
        _characterController.enabled = true;

        _isDestroyed= false;
    }

    private void SetGameplayEnabled(bool enabled)
    {
        foreach (Behaviour component in _gameplayComponents)
        {
            if(component!= null)
            {
                component.enabled = enabled;
            }
        }
    }
}
