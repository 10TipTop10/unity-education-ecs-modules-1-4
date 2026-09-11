using UnityEngine;

public class HealingRingEffect : MonoBehaviour
{
    [SerializeField] private Transform _ringTransform;
    [SerializeField] private MeshRenderer _ringRenderer;
    [SerializeField] private float _duration = 0.8f;
    [SerializeField] private float _startScale = 0.2f;
    [SerializeField] private float _endScale = 3f;
    [SerializeField] private float _elapsedTime;
    [SerializeField] private float _effectStrength;
    private MaterialPropertyBlock _ringBlock;

    private void Awake()
    {
        _ringTransform.localScale = Vector3.one * _startScale;
        _ringBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        float progress = Mathf.Clamp(_elapsedTime / _duration, 0, 1);
        _effectStrength = Mathf.Clamp(1 - progress, 0, 1);
        _ringBlock.SetFloat("_EffectStrength", _effectStrength);
        _ringRenderer.SetPropertyBlock(_ringBlock);


        _ringTransform.localScale = Vector3.one * Mathf.Lerp(_startScale, _endScale, progress);
        if (progress >= 1)
        {
            Destroy(gameObject);
        }
    }
}
