using System;
using UnityEngine;
using UnityEngine.UI;

public class DrugInBloodUI : MonoBehaviour
{
    [SerializeField] private Slider _ketamineSlider;
    [SerializeField] private Slider _vitaminSlider;

    private void Start()
    {
        //sub to events
        PlayerVitalStats.OnPlayerHealthStateChanged += UpdateUI;
    }

    private void UpdateUI(PlayerVitalStats playerVitalStats, PlayerHealthState state)
    {
        var ketamineValue = playerVitalStats.KetamineInBlood / 100;
        _ketamineSlider.value = ketamineValue;
        
        var vitaminValue = playerVitalStats.VitaminInBlood / 100;
        _vitaminSlider.value = vitaminValue;
    }
}
