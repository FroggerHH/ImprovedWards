using UnityEngine;

namespace ImprovedWards;

public static class Const
{
    public static readonly int wardPrefabName = "guard_stone".GetStableHashCode();
    public static readonly int hammerPrefabName = "Hammer".GetStableHashCode();
    public static readonly int haldorLocationName = "Vendor_BlackForest".GetStableHashCode();
    public static readonly string noMonsterAreaObjName = "NoMonsterArea";
    public static Color wardShieldColor = new(0, 0.2f, 0.5f, 1);
    public static EffectList.EffectData[] effectPrefabs;

}