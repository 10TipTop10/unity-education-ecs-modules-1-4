using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MissileStatusView : MonoBehaviour
{
    [SerializeField] private ProjectileWeapon _weapon;
    [SerializeField] private Image[] _missileFills;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private TMP_Text _stateText;

    private void OnEnable()
    {
        _weapon.AmmoStateChanged += UpdateView;
        UpdateView();
    }

    private void Start()
    {
        UpdateView();
    }

    private void UpdateView()
    {
        int remaining = _weapon.MissilesRemaining;

        for (int i = 0; i < _missileFills.Length; i++)
        {
            _missileFills[i].enabled = i < remaining;
        }

        _countText.text = $"{remaining}/{_weapon.MagazineSize}";
        _stateText.text = _weapon.IsReloading ? "ПЕРЕЗАРЯДКА" : "ГОТОВ";
    }

    private void OnDisable()
    {
        _weapon.AmmoStateChanged -= UpdateView;
    }
}
