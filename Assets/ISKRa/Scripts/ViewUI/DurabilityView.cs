using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DurabilityView : MonoBehaviour
{
    [SerializeField] private Durability _durability;
    [SerializeField] private Image[] _segmentFills;
    [SerializeField] private TMP_Text _text;

    private Color _healthyColor = new Color32(185, 232, 242, 255);
    private Color _warningColor = new Color32(242, 190, 70, 255);
    private Color _criticalColor = new Color32(235, 70, 70, 255);
    private Coroutine _fillCoroutine;
    private const float FillAnimationDuration = 0.2f;
    private float _displayedDurability;
    private bool _isViewInitialized;
    private bool _hasStarted;

    private void OnEnable()
    {
        _durability.OnDurabilityChange += UpdateView;

        if (_hasStarted)
        {
            InitializeView();
        }
    }

    private void Start()
    {
        _hasStarted = true;
        InitializeView();
    }

    private void InitializeView()
    {
        UpdateView(_durability.CurrentDurability, _durability.MaxDurability);
    }

    private void UpdateView(int currentDurability, int maxDurability)
    {
        float targetDurability = maxDurability > 0 ? Mathf.Clamp01((float)currentDurability / maxDurability) : 0f;

        int percentage = Mathf.RoundToInt(targetDurability * 100f);
        _text.text = $"{percentage}%";

        _text.color = GetFillColor(targetDurability);

        if (!_isViewInitialized)
        {
            _displayedDurability = targetDurability;
            _isViewInitialized = true;

            UpdateSegments(_displayedDurability);
            return;
        }

        if(_fillCoroutine != null)
        {
            StopCoroutine(_fillCoroutine);
        }

        _fillCoroutine = StartCoroutine(AnimateFill(targetDurability));

    }

    private void UpdateSegments(float normalizedDurability)
    {
        float totalFill = normalizedDurability * _segmentFills.Length;

        Color fillColor = GetFillColor(normalizedDurability);

        for (int i = 0; i < _segmentFills.Length; i++)
        {
            float segmentFill = Mathf.Clamp01(totalFill - i);

            Image fillImage = _segmentFills[i];
            RectTransform fillRect = fillImage.rectTransform;

            Vector2 anchorMax = fillRect.anchorMax;
            anchorMax.x = segmentFill;
            fillRect.anchorMax = anchorMax;

            fillImage.color = fillColor;

        }
    }

    private IEnumerator AnimateFill(float targetDurability)
    {
        float startDurability = _displayedDurability;
        float elapsedTime = 0f;

        while (elapsedTime < FillAnimationDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / FillAnimationDuration);

            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            _displayedDurability = Mathf.Lerp(startDurability, targetDurability, smoothProgress);

            UpdateSegments(_displayedDurability);

            yield return null;
        }

        _displayedDurability = targetDurability;
        UpdateSegments(_displayedDurability);

        _fillCoroutine = null;
    }

    private Color GetFillColor(float normalizedDurability)
    {
        if (normalizedDurability >= 0.6f)
        {
            return _healthyColor;
        }

        if (normalizedDurability >= 0.3f)
        {
            return _warningColor;
        }

        return _criticalColor;
    }

    private void OnDisable()
    {
        _durability.OnDurabilityChange -= UpdateView;

        if(_fillCoroutine != null)
        {
            StopCoroutine(_fillCoroutine);
            _fillCoroutine = null;
        }
    }
}
