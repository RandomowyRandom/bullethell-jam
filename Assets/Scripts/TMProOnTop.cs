using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMProOnTop : MonoBehaviour
{
    [SerializeField] private int _sortingOrder;
    void Start()
    {
        GetComponent<TextMeshPro>().sortingOrder = _sortingOrder;
    }
}
