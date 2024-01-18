using HarmonyLib;

namespace WallJumpHUD.Patches
{
    [HarmonyPatch(typeof(WeaponHUD), "Awake")]
    public class WeaponHUD_Awake_Patch
    {
        public static void Postfix(WeaponHUD __instance)
        {
            __instance.gameObject.AddComponent<WallJumpWeaponController>();
        }
    }
}
