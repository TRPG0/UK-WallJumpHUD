using HarmonyLib;

namespace WallJumpHUD.Patches
{
    [HarmonyPatch(typeof(PlayerActivatorRelay), "Activate")]
    public class PlayerActivatorRelay_Activate_Patch
    {
        public static void Postfix()
        {
            StatsManager.Instance.crosshair?.GetOrAddComponent<WallJumpCrosshairController>();
        }
    }
}
