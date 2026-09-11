using UnityEngine;

public class HealingEffectManager : MonoBehaviour
{
    [SerializeField] private GameObject _effectGameObject;
    [SerializeField] private PlayerEntityBridge _playerEntityBridge;

    public void PlayHealingEffect()
    {
        if (_playerEntityBridge.TryGetPlayerPosition(out Vector3 PlayerPosition))
        {
            Instantiate(_effectGameObject, PlayerPosition, Quaternion.identity);
        }
    }
}
