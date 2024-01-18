using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HarmonyLib;
using System.ComponentModel;

namespace WallJumpHUD
{
    public class WallJumpWeaponController : MonoBehaviour
    {
        public static WallJumpWeaponController Instance { get; private set; }

        public Image icon;
        public TextMeshProUGUI text;

        private void Start()
        {
            if (Instance != null && Instance != this) return;
            Instance = this;

            TMP_FontAsset font = transform.parent.parent.parent.GetComponentInChildren<TextMeshProUGUI>().font;

            GameObject iconO = new GameObject();
            iconO.name = "WallJumpsIcon";
            iconO.layer = 5;
            iconO.transform.SetParent(transform);
            iconO.transform.localPosition = new Vector3(85, -35, 0);
            iconO.transform.localRotation = new Quaternion();
            iconO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            icon = iconO.AddComponent<Image>();
            icon.sprite = Core.bundle.LoadAsset<Sprite>("assets/icon.png");
            icon.color = Core.WeaponColor;

            GameObject textO = new GameObject();
            textO.name = "WallJumpsText";
            textO.layer = 5;
            textO.transform.SetParent(transform);
            textO.transform.localPosition = new Vector3(65, -34, 0);
            textO.transform.localRotation = new Quaternion();
            textO.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            text = textO.AddComponent<TextMeshProUGUI>();
            text.font = font;
            text.alignment = TextAlignmentOptions.Center;
            text.text = Core.MaxWalljumps.ToString();
            text.color = Core.WeaponColor;

            SetStuffActive(ConfigManager.weaponShow.value);
            NewMovementListener.OnWallJumpsChanged += SetWallJumps;
            PowerUpMeterListener.OnPowerUpStarted += OnPowerUpChange;
            PowerUpMeterListener.OnPowerUpEnded += OnPowerUpChange;
        }

        private void OnDestroy()
        {
            NewMovementListener.OnWallJumpsChanged -= SetWallJumps;
            PowerUpMeterListener.OnPowerUpStarted -= OnPowerUpChange;
            PowerUpMeterListener.OnPowerUpEnded -= OnPowerUpChange;
        }

        public void SetWallJumps(int number)
        {
            if (text == null) return;
            if (number > Core.MaxWalljumps) number = Core.MaxWalljumps;
            text.text = number.ToString();
        }

        public void OnPowerUpChange()
        {
            SetWallJumps(NewMovement.Instance.gc.onGround ? Core.MaxWalljumps : NewMovement.Instance.currentWallJumps);
        }

        public void UpdateColor()
        {
            if (icon == null || text == null) return;

            icon.color = Core.WeaponColor;
            text.color = Core.WeaponColor;
        }

        public void SetStuffActive(bool active)
        {
            if (icon == null || text == null) return;

            icon.gameObject.SetActive(active);
            text.gameObject.SetActive(active);
        }
    }
}
