using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace WallJumpHUD
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency("com.eternalUnion.pluginConfigurator")]
    [BepInDependency("trpg.archipelagoultrakill", BepInDependency.DependencyFlags.SoftDependency)]
    public class Core : BaseUnityPlugin
    {
        public const string PluginGUID = "trpg.uk.walljumphud";
        public const string PluginName = "WallJumpHUD";
        public const string PluginVersion = "1.0.0";

        public static string workingPath;
        public static string workingDir;

        public static new ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("WallJumpHUD");

        public static AssetBundle bundle = AssetBundle.LoadFromMemory(Properties.Resources.trpg_walljumphud);

        public static bool IsArchipelagoLoaded { get; private set; }

        public static int MaxWalljumps
        {
            get
            {
                if (IsArchipelagoLoaded) return GetArchipelagoWallJumps();
                return 3;
            }
        }

        public static Color WeaponColor
        {
            get
            {
                if (ConfigManager.weaponMatch.value) return ColorBlindSettings.Instance.staminaColor;
                else return ConfigManager.weaponColor.value;
            }
        }

        public static Color CrosshairColor
        {
            get
            {
                if (ConfigManager.crosshairMatch.value) return ColorBlindSettings.Instance.staminaColor;
                else return ConfigManager.crosshairColor.value;
            }
        }

        private void Awake()
        {
            Harmony Harmony = new Harmony("WallJumpHUD");
            Harmony.PatchAll();

            workingPath = Assembly.GetExecutingAssembly().Location;
            workingDir = Path.GetDirectoryName(workingPath);

            ConfigManager.Init();

            IsArchipelagoLoaded = false;
            foreach (var plugin in Chainloader.PluginInfos)
            {
                if (plugin.Value.Metadata.GUID == "trpg.archipelagoultrakill")
                {
                    IsArchipelagoLoaded = true;
                    Logger.LogInfo("Archipelago is loaded.");
                    break;
                }
            }
        }

        private static int GetArchipelagoWallJumps()
        {
            if (ArchipelagoULTRAKILL.Components.PlayerHelper.CurrentPowerup == ArchipelagoULTRAKILL.Structures.Powerup.WalljumpLimiter) return 0; 
            if (ArchipelagoULTRAKILL.Core.DataExists()) return ArchipelagoULTRAKILL.Core.data.walljumps;
            return 3;
        }
    }
}
