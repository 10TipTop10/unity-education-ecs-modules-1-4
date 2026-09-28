using TMPro;
using UnityEngine;

public class ScrapCounterView : MonoBehaviour
{
    [SerializeField] private ScrapStorage _scrapStorage;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _scrapStorage.ScrapChanged += UpdateText;
        UpdateText(_scrapStorage.CurrentScrap);
    }

    private void UpdateText(int amount)
    {
        _text.text = amount.ToString();
    }

    private void OnDisable()
    {
        _scrapStorage.ScrapChanged -= UpdateText;
    }
}
