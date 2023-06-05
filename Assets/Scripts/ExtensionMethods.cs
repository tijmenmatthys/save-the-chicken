using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ExtentionMethods
{
    private static System.Random random = new System.Random();
    /// <summary>
    /// Check whether the layermask contains a certain layer
    /// </summary>
    public static bool Contains(this LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> array)
    {
        return array.OrderBy(x => random.Next());
    }
}
