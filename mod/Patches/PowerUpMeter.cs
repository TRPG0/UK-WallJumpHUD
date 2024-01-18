using HarmonyLib;

namespace WallJumpHUD.Patches
{
    [HarmonyPatch(typeof(PowerUpMeter), "Start")]
    public class PowerUpMeter_Start_Patch
    {
        public static void Postfix(PowerUpMeter __instance)
        {
            __instance.gameObject.AddComponent<PowerUpMeterListener>();
        }
    }
}
