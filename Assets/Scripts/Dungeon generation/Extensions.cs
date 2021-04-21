using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class Extensions
{
    private static Random rng = new Random();

    private static Camera _mainCamera;
    public static Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;

            return _mainCamera;
        }
    }
    
    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
    
    public static T GetRandomElement<T>(this IList<T> list)
    {
        return list[rng.Next(list.Count)];
    }
}