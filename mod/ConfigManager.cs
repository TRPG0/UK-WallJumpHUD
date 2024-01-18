using PluginConfig.API;
using PluginConfig.API.Fields;
using PluginConfig.API.Decorators;
using System.IO;
using UnityEngine;

namespace WallJumpHUD
{
    public class ConfigManager
    {
        public static PluginConfigurator config = null;

        public static BoolField weaponShow;
        public static BoolField weaponMatch;
        public static ColorField weaponColor;

        public static BoolField crosshairShow;
        public static EnumField<CrosshairAlignment> crosshairAlignment;
        public static BoolField crosshairMatch;
        public static ColorField crosshairColor;

        public static void Init()
        {
            if (config != null) return;

            config = PluginConfigurator.Create("WallJumpHUD", Core.PluginGUID);

            string iconPath = Path.Combine(Core.workingDir, "icon.png");
            if (File.Exists(iconPath)) config.SetIconWithURL(iconPath);

            new ConfigHeader(config.rootPanel, "--WEAPON HUD--");
            new ConfigHeader(config.rootPanel, "Weapon HUD icon is only visible if HUD type is Standard.", 12);
            weaponShow = new BoolField(config.rootPanel, "SHOW ON WEAPON HUD", "weaponShow", true);
            weaponShow.postValueChangeEvent += (bool e) =>
            {
                if (WallJumpWeaponController.Instance != null) WallJumpWeaponController.Instance.SetStuffActive(e);
            };

            weaponMatch = new BoolField(config.rootPanel, "MATCH STAMINA COLOR", "weaponMatch", false);
            weaponMatch.postValueChangeEvent += (bool e) =>
            {
                if (WallJumpWeaponController.Instance != null) WallJumpWeaponController.Instance.UpdateColor();
            };

            weaponColor = new ColorField(config.rootPanel, "WEAPON HUD COLOR", "weaponColor", Color.white);
            weaponColor.postValueChangeEvent += (Color e) =>
            {
                if (WallJumpWeaponController.Instance != null) WallJumpWeaponController.Instance.UpdateColor();
            };

            new ConfigHeader(config.rootPanel, "--CROSSHAIR HUD--");
            crosshairShow = new BoolField(config.rootPanel, "SHOW ON CROSSHAIR HUD", "crosshairShow", true);
            crosshairShow.postValueChangeEvent += (bool e) =>
            {
                if (WallJumpCrosshairController.Instance != null) WallJumpCrosshairController.Instance.SetIconsActive(e);
            };

            crosshairAlignment = new EnumField<CrosshairAlignment>(config.rootPanel, "ALIGNMENT", "crosshairAlignment", CrosshairAlignment.Right);
            crosshairAlignment.postValueChangeEvent += (CrosshairAlignment e) =>
            {
                if (WallJumpCrosshairController.Instance != null)
                {
                    WallJumpCrosshairController.Instance.SetIconsRotation(e);
                    if (PowerUpMeter.Instance != null && PowerUpMeter.Instance.latestMaxJuice > 0) WallJumpCrosshairController.Instance.OnPowerUpStarted();
                }
            };

            crosshairMatch = new BoolField(config.rootPanel, "MATCH STAMINA COLOR", "crosshairMatch", false);
            crosshairColor = new ColorField(config.rootPanel, "CROSSHAIR HUD COLOR", "crosshairColor", Color.white);
        }
    }
}
