using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class FreezingEffect : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _effectMaterial;
    [SerializeField] private float _effectDuration = 3;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    private Material _bufferMaterial;
    private bool _freezingIsActive;

    private void Awake()
    {
        _bufferMaterial = _renderer.sharedMaterial;
    }

    public void Activate()
    {
        if (_freezingIsActive) return;
        _navMeshAgent.isStopped = true;
        _freezingIsActive = true;
        _renderer.sharedMaterial = _effectMaterial;
        StartCoroutine(RestoreMaterialAfterDelay());
    }

    private IEnumerator RestoreMaterialAfterDelay()
    {
        yield return new WaitForSeconds(_effectDuration);

        _renderer.sharedMaterial = _bufferMaterial;
        _navMeshAgent.isStopped = false;
        _freezingIsActive = false;
    }
}
