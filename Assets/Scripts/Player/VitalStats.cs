using UnityEngine;

[System.Serializable]
public class VitalStats
{
    [SerializeField] private float _health;
    [SerializeField] private float _ketamineReduction;
    [SerializeField] private float _vitaminReduction;

    public float Health => _health;
    public float KetamineReduction => _ketamineReduction;
    public float VitaminReduction => _vitaminReduction;
}