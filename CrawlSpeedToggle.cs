using MelonLoader;
using UIExpansionKit.API;
using UnityEngine.Playables;
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
        }

        private static VRCPlayer GetLocalPlayer()
        {
            return VRCPlayer.field_Internal_Static_VRCPlayer_0;
        }

        private static void InitUI()
        {
            ICustomLayoutedMenu quickMenu = ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu);
            quickMenu.AddToggleButton(
                "Crawl Speed", 
                ToggleSpeed, 
                () => _togglePreference.Value, 
                null
                );
        }

        private static void ToggleSpeed(bool val)
        {
            VRCPlayerApi player = GetLocalPlayer().prop_VRCPlayerApi_0;
            _togglePreference.Value = val;

            if (val)
            {
                _oldRunSpeed = player.GetRunSpeed();
                _oldWalkSpeed = player.GetWalkSpeed();
                _oldStrafeSpeed = player.GetStrafeSpeed();
                
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
    }
}