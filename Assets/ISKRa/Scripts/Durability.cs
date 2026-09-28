using System;
using UnityEngine;

public class Durability : MonoBehaviour
{
    [SerializeField] private int _maxDurability = 100;

    public int MaxDurability => _maxDurability;
    public event Action<int, int> OnDurabilityChange;
    public int CurrentDurability { get; private set; }
    public event Action Depleted;

    private void Awake()
    {
        CurrentDurability = _maxDurability;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        if (CurrentDurability <= 0) return;

        CurrentDurability = Mathf.Max(CurrentDurability - amount, 0);
        
        OnDurabilityChange?.Invoke(CurrentDurability, _maxDurability);

        if(CurrentDurability<= 0)
        {
            Depleted?.Invoke();
        }

    }

    public void Repair(int amount)
    {
        if (amount <= 0) return;
        if (CurrentDurability >= _maxDurability) return;

        CurrentDurability = Mathf.Min(CurrentDurability + amount, _maxDurability);

        OnDurabilityChange?.Invoke(CurrentDurability, _maxDurability);
    }

    public void RestoreFull()
    {
        Repair(_maxDurability - CurrentDurability);
    }
}
