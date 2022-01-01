using System.Data.SqlTypes;
using Il2CppSystem;
using MelonLoader;
using UIExpansionKit.API;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRC.Core;
using VRC.SDKBase;

namespace CorneliusCornbread
{
    public class CrawlSpeedToggle : MelonMod
    {
        private const string ModCategoryId = "CC_CrawlToggle";
        private static MelonPreferences_Category _modCategory;
        private static MelonPreferences_Entry<bool> _togglePreference;

        private static float _oldRunSpeed, _oldWalkSpeed, _oldStrafeSpeed = 3; //Just in case something wigs out, at least you can move.

        /// <summary>
        /// The percentage of speed based on the world's run speed.
        /// </summary>
        private const float _speedMult = .1f;
        
        public static MelonLogger.Instance Logger { get; private set; }
        
        public override void OnApplicationStart()
        {
            Logger = LoggerInstance;
            
            Logger.Msg("---Crawl Speed Toggle Init---");
            
            _modCategory = MelonPreferences.CreateCategory(ModCategoryId, ModBuildInfo.Name);
            _togglePreference = _modCategory.CreateEntry(CTModSetting.CrawlToggle.ToString(), false);
            
            InitUI();

            VRChatUtilityKit.Utilities.NetworkEvents.OnAvatarInstantiated += OnPlayerInstantiate;
        }

        private static void InitUI()
        {
            ICustomLayoutedMenu quickMenu = ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu);

            quickMenu.AddToggleButton(
                "Crawl Speed", 
                ToggleSpeed, 
                GetToggleState
            );
        }

        private static void OnPlayerInstantiate(VRCAvatarManager avMan, ApiAvatar avApi, GameObject gObj)
        {
            VRCPlayerApi player = GetLocalPlayer().prop_VRCPlayerApi_0;
            
            UpdateSpeed(player);
            
            ToggleSpeed(_togglePreference.Value, player);
        }

        private static VRCPlayer GetLocalPlayer()
        {
            return VRCPlayer.field_Internal_Static_VRCPlayer_0;
        }

        private static bool GetToggleState()
        {
            return _togglePreference.Value;
        }

        private static void ToggleSpeed(bool val)
        {
            ToggleSpeed(val, GetLocalPlayer().prop_VRCPlayerApi_0);
        }
        
        private static void ToggleSpeed(bool val, VRCPlayerApi player)
        {
            _togglePreference.Value = val;

            if (val)
            {
                UpdateSpeed();
                
                float _newSpeed = _oldRunSpeed * _speedMult;
                player.SetRunSpeed(_newSpeed);
                player.SetStrafeSpeed(_newSpeed);
                player.SetWalkSpeed(_newSpeed);
            }
            else
            {
                player.SetRunSpeed(_oldRunSpeed);
                player.SetWalkSpeed(_oldStrafeSpeed);
                player.SetStrafeSpeed(_oldWalkSpeed);
            }
        }

        private static void UpdateSpeed(VRCPlayerApi player = null)
        {
            if (player == null)
            {
                player = GetLocalPlayer().prop_VRCPlayerApi_0;
            }

            //If you join a world that doesn't change the speed or is the same speed, the player's speed will not be
            //updated. So we have to do this janky shit for our 'old speed' to be correct.
            if (Mathf.Abs((_oldRunSpeed * _speedMult) - player.GetRunSpeed()) < .05f)
            {
                return;
            }
            
            _oldRunSpeed = player.GetRunSpeed();
            _oldWalkSpeed = player.GetWalkSpeed();
            _oldStrafeSpeed = player.GetStrafeSpeed();
        }
    }
}