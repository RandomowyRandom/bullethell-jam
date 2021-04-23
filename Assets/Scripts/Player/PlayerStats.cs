using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static event Action<PlayerStats> OnPlayerStatsChanged;
    
    [SerializeField] private Stats _baseStats;
    [SerializeField] private Stats _playerStats;

    public Stats Stats => _playerStats;

    private void Start()
    {
        // sub to events
        PlayerInventory.OnItemsInInventoryChanged += UpdateStats;
        UpdateStats(GetComponent<PlayerInventory>());
    }

    private void UpdateStats(PlayerInventory obj)
    {
        // new stats based on base stats
        var newStats = new Stats
        {
            Speed = _baseStats.Speed,
            Damage = _baseStats.Damage,
            FireRate = _baseStats.FireRate,
            Range = _baseStats.Range,
            DrugEfficiency = _baseStats.DrugEfficiency,
            BulletSpeed = _baseStats.BulletSpeed
        };

        // iterate throughout each item and include it's stats
        foreach (var item in obj.Items)
        {
            newStats.Speed += item.Stats.Speed;
            newStats.Damage += item.Stats.Damage;
            newStats.FireRate += item.Stats.FireRate;
            newStats.Range += item.Stats.Range;
            newStats.DrugEfficiency += item.Stats.DrugEfficiency;
            newStats.BulletSpeed += item.Stats.BulletSpeed;
        }

        _playerStats = newStats;
        OnPlayerStatsChanged?.Invoke(this);
    }

    private void OnDestroy()
    {
        PlayerInventory.OnItemsInInventoryChanged -= UpdateStats;
    }
}
