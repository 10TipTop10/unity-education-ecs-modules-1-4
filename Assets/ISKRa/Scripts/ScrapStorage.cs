using System;
using UnityEngine;

public class ScrapStorage : MonoBehaviour
{
    [SerializeField] private int _startingScrap = 0;

    public int CurrentScrap { get; private set; }

    public event Action<int> ScrapChanged;

    private void Awake()
    {
        CurrentScrap = _startingScrap;
    }

    public void AddScrap(int amount)
    {
        if (amount <= 0) return;

        CurrentScrap += amount;
        ScrapChanged?.Invoke(CurrentScrap);
    }

    public bool TrySpendScrap(int amount)
    {
        if(amount<= 0) return false;
        if (CurrentScrap < amount) return false;

        CurrentScrap -= amount;
        ScrapChanged?.Invoke(CurrentScrap);

        return true;
    }
}
