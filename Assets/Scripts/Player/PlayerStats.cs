using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Stats _baseStats;
    [SerializeField] private Stats _playerStats;

    public Stats Stats => _playerStats;

    private void Start()
    {
        // sub to events
        PlayerInventory.OnItemsInInventoryChanged += UpdateStats;
        _playerStats = _baseStats;
    }

    private void UpdateStats(PlayerInventory obj)
    {
        // new stats based on base stats
        var newStats = new Stats()
        {
            Speed = _baseStats.Speed,
            Damage = _baseStats.Damage,
            FireRate = _baseStats.FireRate,
            Range = _baseStats.Range,
            DrugTolerance = _baseStats.DrugTolerance,
            DrugEfficiency = _baseStats.DrugEfficiency
        };

        // iterate throughout each item and include it's stats
        foreach (var item in obj.Items)
        {
            newStats.Speed += item.Stats.Speed;
            newStats.Damage += item.Stats.Damage;
            newStats.FireRate += item.Stats.FireRate;
            newStats.Range += item.Stats.Range;
            newStats.DrugTolerance += item.Stats.DrugTolerance;
            newStats.DrugEfficiency += item.Stats.DrugEfficiency;
        }

        _playerStats = newStats;
    }

    private void OnDestroy()
    {
        PlayerInventory.OnItemsInInventoryChanged -= UpdateStats;
    }
}
