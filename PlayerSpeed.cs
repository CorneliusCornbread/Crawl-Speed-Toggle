using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using VRChatUtilityKit.Utilities;

namespace CorneliusCornbread.CrawlToggle
{
    public static class PlayerSpeed
    {
        private static float _oldRunSpeed, _oldWalkSpeed, _oldStrafeSpeed = 3; //Just in case something wigs out, at least you can move.

        public static void Init()
        {
            NetworkEvents.OnAvatarInstantiated += OnPlayerInstantiate;

            CTPreferences.CrawlTogglePref.OnValueChanged += ToggleCrawlSpeed;
            CTPreferences.WalkTogglePref.OnValueChanged += ToggleWalkSpeed;
        }

        public static void DisableSpeedModifiers(VRCPlayerApi player = null)
        {
            if (player == null)
            {
                player = CornUtils.GetLocalPlayerApi();
            }

            player.SetRunSpeed(_oldRunSpeed);
            player.SetStrafeSpeed(_oldStrafeSpeed);
            player.SetWalkSpeed(_oldWalkSpeed);
        }

        #region Crawl Speed
        public static void ToggleCrawlSpeed(bool val, VRCPlayerApi player)
        {
            CTPreferences.CrawlTogglePref.Value = val;

            if (val)
            {
                float newSpeed = CTPreferences.CrawlSpeedPref.Value;
                
                SetSpeed(newSpeed, player);
            }
            else
            {
                ResetSpeed(player);
            }
        }
        
        public static void ToggleCrawlSpeed(bool oldVal, bool newVal)
        {
            if (newVal)
            {
                CTPreferences.WalkTogglePref.Value = false;
            }
            
            ToggleCrawlSpeed(newVal, CornUtils.GetLocalPlayerApi());
        }
        #endregion

        #region Walk Speed
        public static void ToggleWalkSpeed(bool val, VRCPlayerApi player)
        {
            CTPreferences.WalkTogglePref.Value = val;

            if (val)
            {
                float newSpeed = CTPreferences.WalkSpeedPref.Value;
                
                SetSpeed(newSpeed, player);
            }
            else
            {
                ResetSpeed(player);
            }
        }
        
        public static void ToggleWalkSpeed(bool oldVal, bool newVal)
        {
            if (newVal)
            {
                CTPreferences.CrawlTogglePref.Value = false;
            }
            
            ToggleWalkSpeed(newVal, CornUtils.GetLocalPlayerApi());
        }
        #endregion
        
        private static void OnPlayerInstantiate(VRCAvatarManager avMan, ApiAvatar avApi, GameObject gObj)
        {
            VRCPlayerApi player = CornUtils.GetLocalPlayerApi();
            
            CopyDefaultSpeed(player);
            
            ToggleCrawlSpeed(CTPreferences.CrawlTogglePref.Value, player);
        }
        
        private static void CopyDefaultSpeed(VRCPlayerApi player = null)
        {
            if (player == null)
            {
                player = CornUtils.GetLocalPlayerApi();
            }

            //If you join a world that doesn't change the speed or is the same speed, the player's speed will not be
            //updated. So we have to do this janky shit for our 'old speed' to be correct.
            if (GetOldMaxSpeed() == GetCurrentMaxSpeed(player))
            {
                return;
            }
            
            _oldRunSpeed = player.GetRunSpeed();
            _oldWalkSpeed = player.GetWalkSpeed();
            _oldStrafeSpeed = player.GetStrafeSpeed();
        }

        private static void SetSpeed(float speed, VRCPlayerApi player)
        {
            DisableSpeedModifiers(player); // Don't stack speeds
            CopyDefaultSpeed(); //Regrab our speeds

            float maxSpeed = GetOldMaxSpeed();

            speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
            
            player.SetRunSpeed(speed);
            //Strafe speed seems to be special in the way its calculated for some reason
            player.SetStrafeSpeed(Mathf.Clamp(speed, -_oldStrafeSpeed, _oldStrafeSpeed)); 
            player.SetWalkSpeed(speed);
        }

        private static void ResetSpeed(VRCPlayerApi player)
        {
            player.SetRunSpeed(_oldRunSpeed);
            player.SetWalkSpeed(_oldWalkSpeed);
            player.SetStrafeSpeed(_oldStrafeSpeed);
        }
        
        private static float GetOldMaxSpeed()
        {
            return Mathf.Max(_oldRunSpeed, _oldWalkSpeed);
        }

        private static float GetCurrentMaxSpeed(VRCPlayerApi player = null)
        {
            if (player == null)
            {
                player = CornUtils.GetLocalPlayerApi();
            }

            return Mathf.Max(player.GetRunSpeed(), player.GetWalkSpeed());
        }
    }
}