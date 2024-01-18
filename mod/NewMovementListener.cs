using System;
using UnityEngine;

namespace WallJumpHUD
{
    public class NewMovementListener : MonoBehaviour
    {
        public static NewMovementListener Instance { get; private set; }

        public static event OnWallJumpsChangedDelegate OnWallJumpsChanged;

        public delegate void OnWallJumpsChangedDelegate(int number);

        private int previous;

        private void Awake()
        {
            if (Instance != null && Instance != this) return;
            Instance = this;
            previous = NewMovement.Instance.currentWallJumps;
        }

        private void Update()
        {
            if (previous != NewMovement.Instance.currentWallJumps)
            {
                if (OnWallJumpsChanged != null) OnWallJumpsChanged(3 - NewMovement.Instance.currentWallJumps);
                previous = NewMovement.Instance.currentWallJumps;
                //Core.Logger.LogInfo($"Wall jumps changed. {previous}");
            }
        }
    }
}
