using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace ImprovedWards;

public static class TerminalCommands
{
    private static bool isServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
    private static string modName => Plugin.ModName;


    [HarmonyPatch(typeof(Terminal), nameof(Terminal.InitTerminal))]
    [HarmonyWrapSafe]
    internal class AddChatCommands
    {
        private static void Postfix()
        {
        }
    }
}