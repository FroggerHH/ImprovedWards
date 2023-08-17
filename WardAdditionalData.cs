using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ImprovedWards;

[Serializable]
public class WardAdditionalData
{
    public MeshRenderer shield;
    public GameObject noMonsterArea;

    public WardAdditionalData()
    {
        shield = null;
        noMonsterArea = null;
    }
}

public static class WardExtension
{
    private static readonly ConditionalWeakTable<PrivateArea, WardAdditionalData> data = new();

    public static WardAdditionalData GetAdditionalData(this PrivateArea privateArea)
    {
        return data.GetOrCreateValue(privateArea);
    }

    public static void AddData(this PrivateArea privateArea, WardAdditionalData value)
    {
        try
        {
            data.Add(privateArea, value);
        }
        catch
        {
        }
    }
}