using System.Linq;
using Extensions;
using HarmonyLib;
using UnityEngine;
using static ImprovedWards.Plugin;
using static UnityEngine.Object;
using static ImprovedWards.Const;

namespace ImprovedWards.Patch;

[HarmonyPatch]
public static class InitWards
{
    private static Transform wardsHolder;
    private static bool inited;

    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.SpawnZone))]
    [HarmonyWrapSafe]
    [HarmonyPostfix]
    private static void CreateWards()
    {
        if (inited) return;
        inited = true;

        var haldor = ZoneSystem.instance.GetLocation(haldorLocationName);
        var shield = haldor.m_prefab.transform.FindChildByName("ForceField").gameObject;

        Debug("Creating wards...");

        var vanilaWard = ZNetScene.instance.GetPrefab(wardPrefabName);
        wardsHolder = new GameObject("WardsHolder").transform;
        DontDestroyOnLoad(wardsHolder);
        wardsHolder.gameObject.SetActive(false);

        var newWard = Instantiate(vanilaWard, wardsHolder);
        newWard.name = vanilaWard.name;
        var newShield = Instantiate(shield, newWard.transform);
        newShield.name = "WardShield";
        Destroy(newShield.transform.FindChildByName(noMonsterAreaObjName).gameObject);
        var shieldMeshRenderer = newShield.GetComponent<MeshRenderer>();
        shieldMeshRenderer.sharedMaterial.color = wardShieldColor;
        var collider = shieldMeshRenderer.gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        shieldMeshRenderer.gameObject.AddComponent<Pusher>();
        shieldMeshRenderer.gameObject.layer = LayerMask.NameToLayer("character_trigger");

        var privateAreaComponent = newWard.GetComponent<PrivateArea>();
        var data = privateAreaComponent.GetAdditionalData();
        data.shield = shieldMeshRenderer;
        Const.effectPrefabs = privateAreaComponent.m_flashEffect.m_effectPrefabs.Select(x => x).ToArray();
        privateAreaComponent.m_flashEffect.m_effectPrefabs = new EffectList.EffectData[0];

        ZNetScene.instance.m_namedPrefabs[wardPrefabName] = newWard;
        ZNetScene.instance.m_prefabs[ZNetScene.instance.m_prefabs.IndexOf(vanilaWard)] = newWard;
        var piecesInHammer = ZNetScene.instance.GetPrefab(hammerPrefabName).GetComponent<ItemDrop>().m_itemData
            .m_shared.m_buildPieces.m_pieces;
        piecesInHammer[piecesInHammer.IndexOf(vanilaWard)] = newWard;

        Debug("Wards is done");
    }
}