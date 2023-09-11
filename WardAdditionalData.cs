using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ImprovedWards;

[Serializable]
public class WardAdditionalData
{
    public MeshRenderer shield;

    public WardAdditionalData()
    {
        shield = null;
    }
}

public static class WardExtension
{
    private static readonly ConditionalWeakTable<PrivateArea, WardAdditionalData> data = new();

    public static WardAdditionalData GetAdditionalData(this PrivateArea privateArea) => data.GetOrCreateValue(privateArea);
}