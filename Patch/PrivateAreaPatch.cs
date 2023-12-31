﻿using System.Linq;
using Extensions;
using HarmonyLib;
using UnityEngine;
using static ImprovedWards.Const;
using static ImprovedWards.Plugin;
using static EffectList;

namespace ImprovedWards.Patch;

[HarmonyPatch(typeof(PrivateArea))]
public static class PrivateAreaPatch
{
    [HarmonyPatch(nameof(PrivateArea.Awake))]
    [HarmonyPostfix]
    [HarmonyWrapSafe]
    private static void InitWardData(PrivateArea __instance)
    {
        var data = __instance.GetAdditionalData();
        if (!data.shield) return;
        data.shield =
            __instance.transform.FindChildByName("WardShield").GetComponent<MeshRenderer>();

        __instance.m_areaMarker.SetActive(true);
    }

    [HarmonyPatch(nameof(PrivateArea.RPC_FlashShield))]
    [HarmonyPrefix]
    [HarmonyWrapSafe]
    private static void FlashCustomShield(PrivateArea __instance)
    {
        var data = __instance.GetAdditionalData();
        if (!data.shield) return;
        __instance.m_flashAvailable = true;
        data.shield.Flash(Color.red, wardShieldColor);
        if (Utils.DistanceXZ(__instance.transform.position, Player.m_localPlayer.transform.position) >
            __instance.m_radius) __instance.m_flashEffect.m_effectPrefabs = new EffectData[0];
        else
            __instance.m_flashEffect.m_effectPrefabs = Const.effectPrefabs.Select(x => x).ToArray();
    }

    [HarmonyPatch(nameof(PrivateArea.UpdateStatus))]
    [HarmonyPostfix]
    [HarmonyWrapSafe]
    private static void UpdateShieldSize(PrivateArea __instance)
    {
        var data = __instance.GetAdditionalData();
        if (!data.shield) return;
        data.shield.transform.localScale = Vector3.one * __instance.m_radius * 2.06f;
        data.shield.sharedMaterial.color = wardShieldColor;
        __instance.m_areaMarker.m_radius = __instance.m_radius;
    }

    [HarmonyPatch(nameof(PrivateArea.HideMarker))]
    [HarmonyPrefix]
    private static bool DoNotHideAreaMarker(PrivateArea __instance)
    {
        if (!__instance.GetAdditionalData().shield) return true;
        return !permanentRangeCircle;
    }

    [HarmonyPatch(nameof(PrivateArea.ShowAreaMarker))]
    [HarmonyPrefix]
    private static bool ShowOrNotAreaMarker(PrivateArea __instance)
    {
        if (!__instance.GetAdditionalData().shield) return true;
        return useRangeCircle;
    }
}