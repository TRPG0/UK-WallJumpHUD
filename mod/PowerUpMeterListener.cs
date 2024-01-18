using UnityEngine;

namespace WallJumpHUD
{
    public class PowerUpMeterListener : MonoBehaviour
    {
        public static PowerUpMeterListener Instance { get; private set; }

        public static event OnPowerUpStartedDelegate OnPowerUpStarted;
        public static event OnPowerUpEndedDelegate OnPowerUpEnded;

        public delegate void OnPowerUpStartedDelegate();
        public delegate void OnPowerUpEndedDelegate();

        private float previous;

        private void Awake()
        {
            if (Instance != null && Instance != this) return;
            Instance = this;
            previous = 0;
        }

        private void Update()
        {
            if (previous == 0 && previous < PowerUpMeter.Instance.latestMaxJuice)
            {
                if (OnPowerUpStarted != null) OnPowerUpStarted();
                //Core.Logger.LogInfo("OnPowerUpStarted");
            }
            else if (previous > 0 && PowerUpMeter.Instance.latestMaxJuice == 0)
            {
                if (OnPowerUpEnded != null) OnPowerUpEnded();
                //Core.Logger.LogInfo("OnPowerUpEnded");
            }
            previous = PowerUpMeter.Instance.latestMaxJuice;
        }
    }
}
