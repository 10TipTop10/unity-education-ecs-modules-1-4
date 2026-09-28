using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuildSiteView : MonoBehaviour
{
    [SerializeField] private BuildSite _buildSite;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _buildSite.ProgressChanged += UpdateProgress;
        _buildSite.Completed += HideProgress;

        UpdateProgress(_buildSite.PaidScrap, _buildSite.BuildCost);

    }

    private void UpdateProgress(int paid, int cost)
    {
        _text.text = $"СБОРКА: {paid}/{cost}";
    }

    private void HideProgress()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _buildSite.ProgressChanged -= UpdateProgress;
        _buildSite.Completed -= HideProgress;
    }
}
