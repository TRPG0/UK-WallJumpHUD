using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace WallJumpHUD
{
    public class WallJumpCrosshairController : MonoBehaviour
    {
        public static WallJumpCrosshairController Instance { get; private set; }

        public Image crosshair1;
        public Image crosshair2;
        public Image crosshair3;

        public Direction Direction
        {
            get
            {
                if (ConfigManager.crosshairAlignment.value == CrosshairAlignment.Left || ConfigManager.crosshairAlignment.value == CrosshairAlignment.Top) return Direction.Reverse;
                else return Direction.Normal;
            }
        }

        public float time = 2;
        public float maxTime = 2;
        public float minTime = 0;

        public NewMovement nm;
        public Traverse nmT;

        private void Start()
        {
            if (Instance != null && Instance != this) return;
            Instance = this;

            nm = NewMovement.Instance;
            nmT = Traverse.Create(nm);

            GameObject container = new GameObject();
            container.name = "WallJumps";
            container.layer = 5;
            container.transform.SetParent(transform);
            container.transform.localPosition = Vector3.zero;
            container.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            GameObject icon1 = new GameObject();
            icon1.name = "1";
            icon1.layer = 5;
            icon1.transform.SetParent(container.transform);
            icon1.transform.localPosition = Vector3.zero;
            icon1.transform.localScale = Vector3.one;
            crosshair1 = icon1.AddComponent<Image>();

            GameObject icon2 = new GameObject();
            icon2.name = "2";
            icon2.layer = 5;
            icon2.transform.SetParent(container.transform);
            icon2.transform.localPosition = Vector3.zero;
            icon2.transform.localScale = Vector3.one;
            crosshair2 = icon2.AddComponent<Image>();

            GameObject icon3 = new GameObject();
            icon3.name = "3";
            icon3.layer = 5;
            icon3.transform.SetParent(container.transform);
            icon3.transform.localPosition = Vector3.zero;
            icon3.transform.localScale = Vector3.one;
            crosshair3 = icon3.AddComponent<Image>();

            crosshair1.sprite = Core.bundle.LoadAsset<Sprite>("assets/1.png");
            crosshair2.sprite = Core.bundle.LoadAsset<Sprite>("assets/2.png");
            crosshair3.sprite = Core.bundle.LoadAsset<Sprite>("assets/3.png");

            SetIconsRotation(ConfigManager.crosshairAlignment.value);
            SetIconsActive(ConfigManager.crosshairShow.value);
            SetWallJumps(Core.MaxWalljumps);
            NewMovementListener.OnWallJumpsChanged += SetWallJumps;
            PowerUpMeterListener.OnPowerUpStarted += OnPowerUpStarted;
            PowerUpMeterListener.OnPowerUpEnded += OnPowerUpEnded;
        }

        private void OnDestroy()
        {
            NewMovementListener.OnWallJumpsChanged -= SetWallJumps;
            PowerUpMeterListener.OnPowerUpStarted -= OnPowerUpStarted;
            PowerUpMeterListener.OnPowerUpEnded -= OnPowerUpEnded;
        }

        private void Update()
        {
            if (ConfigManager.crosshairShow.value)
            {
                if (nmT.Field<float>("fallTime").Value > 0.25) time = maxTime;
                else if (time > minTime) time -= Time.deltaTime;
                SetIconsOpacity(Mathf.Clamp01(time));
            }
        }

        public void SetIconsActive(bool active)
        {
            if (crosshair1 == null || crosshair2 == null || crosshair3 == null) return;

            crosshair1.gameObject.SetActive(active);
            crosshair2.gameObject.SetActive(active);
            crosshair3.gameObject.SetActive(active);
        }

        public void SetIconsRotation(CrosshairAlignment alignment)
        {
            if (crosshair1 == null || crosshair2 == null || crosshair3 == null) return;

            crosshair1.GetComponent<Transform>().localRotation = new Quaternion();
            crosshair2.GetComponent<Transform>().localRotation = new Quaternion();
            crosshair3.GetComponent<Transform>().localRotation = new Quaternion();

            switch (alignment)
            {
                case CrosshairAlignment.Left:
                    crosshair1.GetComponent<Transform>().Rotate(0, 0, 270, Space.Self);
                    crosshair2.GetComponent<Transform>().Rotate(0, 0, 270, Space.Self);
                    crosshair3.GetComponent<Transform>().Rotate(0, 0, 270, Space.Self);
                    break;
                case CrosshairAlignment.Top:
                    crosshair1.GetComponent<Transform>().Rotate(0, 0, 180, Space.Self);
                    crosshair2.GetComponent<Transform>().Rotate(0, 0, 180, Space.Self);
                    crosshair3.GetComponent<Transform>().Rotate(0, 0, 180, Space.Self);
                    break;
                case CrosshairAlignment.Right:
                    crosshair1.GetComponent<Transform>().Rotate(0, 0, 90, Space.Self);
                    crosshair2.GetComponent<Transform>().Rotate(0, 0, 90, Space.Self);
                    crosshair3.GetComponent<Transform>().Rotate(0, 0, 90, Space.Self);
                    break;
                default: break;
            }
        }

        public void OnPowerUpStarted()
        {
            SetWallJumps(nm.gc.onGround ? Core.MaxWalljumps : nm.currentWallJumps);
            if (crosshair1 == null || crosshair2 == null || crosshair3 == null || !PrefsManager.Instance.GetBool("powerUpMeter", true))  return;

            float radiusDist = 12;
            float parallelDist = 6;

            switch (ConfigManager.crosshairAlignment.value)
            {
                case CrosshairAlignment.Top:
                    crosshair1.GetComponent<Transform>().localPosition = new Vector3(parallelDist, radiusDist, 0);
                    crosshair2.GetComponent<Transform>().localPosition = new Vector3(0, radiusDist, 0);
                    crosshair3.GetComponent<Transform>().localPosition = new Vector3(-parallelDist, radiusDist, 0);
                    break;
                case CrosshairAlignment.Left:
                    crosshair1.GetComponent<Transform>().localPosition = new Vector3(-radiusDist, parallelDist, 0);
                    crosshair2.GetComponent<Transform>().localPosition = new Vector3(-radiusDist, 0, 0);
                    crosshair3.GetComponent<Transform>().localPosition = new Vector3(-radiusDist, -parallelDist, 0);
                    break;
                case CrosshairAlignment.Right:
                    crosshair1.GetComponent<Transform>().localPosition = new Vector3(radiusDist, -parallelDist, 0);
                    crosshair2.GetComponent<Transform>().localPosition = new Vector3(radiusDist, 0, 0);
                    crosshair3.GetComponent<Transform>().localPosition = new Vector3(radiusDist, parallelDist, 0);
                    break;
                case CrosshairAlignment.Bottom:
                    crosshair1.GetComponent<Transform>().localPosition = new Vector3(-parallelDist, -radiusDist, 0);
                    crosshair2.GetComponent<Transform>().localPosition = new Vector3(0, -radiusDist, 0);
                    crosshair3.GetComponent<Transform>().localPosition = new Vector3(parallelDist, -radiusDist, 0);
                    break;
            }
        }

        public void OnPowerUpEnded()
        {
            SetWallJumps(nm.gc.onGround ? Core.MaxWalljumps : nm.currentWallJumps);
            if (crosshair1 == null || crosshair2 == null || crosshair3 == null) return;

            crosshair1.GetComponent<Transform>().localPosition = Vector3.zero;
            crosshair2.GetComponent<Transform>().localPosition = Vector3.zero;
            crosshair3.GetComponent<Transform>().localPosition = Vector3.zero;
        }

        public void SetIconsOpacity(float time)
        {
            if (crosshair1 == null || crosshair2 == null || crosshair3 == null) return;

            Color color = Core.CrosshairColor;
            color.a = time;

            crosshair1.color = color;
            crosshair2.color = color;
            crosshair3.color = color;
        }

        public void SetWallJumps(int jumps)
        {
            if (jumps > Core.MaxWalljumps) jumps = Core.MaxWalljumps;
            if (jumps < 0) jumps = 0;

            switch (jumps)
            {
                case 3:
                    crosshair1.gameObject.SetActive(true);
                    crosshair2.gameObject.SetActive(true);
                    crosshair3.gameObject.SetActive(true);
                    break;
                case 2:
                    if (Direction == Direction.Reverse)
                    {
                        crosshair1.gameObject.SetActive(false);
                        crosshair2.gameObject.SetActive(true);
                        crosshair3.gameObject.SetActive(true);
                    }
                    else
                    {
                        crosshair1.gameObject.SetActive(true);
                        crosshair2.gameObject.SetActive(true);
                        crosshair3.gameObject.SetActive(false);
                    }
                    break;
                case 1:
                    if (Direction == Direction.Reverse)
                    {
                        crosshair1.gameObject.SetActive(false);
                        crosshair2.gameObject.SetActive(false);
                        crosshair3.gameObject.SetActive(true);
                    }
                    else
                    {
                        crosshair1.gameObject.SetActive(true);
                        crosshair2.gameObject.SetActive(false);
                        crosshair3.gameObject.SetActive(false);
                    }
                    break;
                case 0:
                    crosshair1.gameObject.SetActive(false);
                    crosshair2.gameObject.SetActive(false);
                    crosshair3.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
