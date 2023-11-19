using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IEnumerableExtensinon 
{
    public static int CenterIndex<T>(this IEnumerable<T> source)
    {
        var length = source.Count();
        return length <= 0 ? 0 : Mathf.CeilToInt((float)length / 2f);
    }
}