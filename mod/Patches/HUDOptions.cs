using HarmonyLib;

namespace WallJumpHUD.Patches
{
    [HarmonyPatch(typeof(HUDOptions), "Start")]
    public class HUDOptions_Start_Patch
    {
        public static void Postfix(HUDOptions __instance)
        {
            if (__instance.gameObject.GetComponentInChildren<Crosshair>(true)) __instance.gameObject.GetComponentInChildren<Crosshair>(true).gameObject.AddComponent<WallJumpCrosshairController>();
        }
    }
}
