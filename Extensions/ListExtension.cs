using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions;

public static class ListExtension
{
    public static T Random<T>(this List<T> list)
    {
        if (list == null || list.Count == 0) return default;
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T Random<T>(this T[] array)
    {
        if (array == null || array.Length == 0) return default;
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    public static List<Vector3> RoundCords(this List<Vector3> list)
    {
        var result = new List<Vector3>();
        for (var i = 0; i < list.Count; i++) result.Add(new Vector3((int)list[i].x, (int)list[i].y, (int)list[i].z));

        return result;
    }

    public static T Next<T>(this List<T> list, T current)
    {
        if (list == null || list.Count == 0) return default;

        if (list.Last().Equals(current)) return list.First();
        return list[list.IndexOf(current) + 1];
    }

    public static string GetJoinedString<T>(this List<T> list, string separator = ", ")
    {
        if (list == null || list.Count == 0) return default;

        return string.Join(separator, list);
    }
}