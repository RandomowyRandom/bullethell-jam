using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;

    private void Start()
    {
        // sub to events
        PlayerVitalStats.OnPlayerHealthChanged += UpdateUI;
    }

    private void UpdateUI(PlayerVitalStats obj)
    {
        var value = obj.Health / obj.MaxHealth;
        _healthSlider.value = value;
    }

    private void OnDestroy()
    {
        PlayerVitalStats.OnPlayerHealthChanged -= UpdateUI;
    }
}
