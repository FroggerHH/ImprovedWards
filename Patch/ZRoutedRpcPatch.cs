using HarmonyLib;

namespace ImprovedWards.Patch;

[HarmonyPatch(typeof(ZNetScene))]
public static class ZRoutedRpcPatch
{
    [HarmonyPatch(nameof(ZNetScene.Awake))]
    private static void Postfix()
    {
        try
        {
            // ZRoutedRpc.instance.Register<Vector2, Vector2i>(nameof(Relocator.RelocateMerchant),
            //    new Action<long, Vector2, Vector2i>(Relocator.RPC_RelocateMerchant));
        }
        catch
        {
        }
    }
}