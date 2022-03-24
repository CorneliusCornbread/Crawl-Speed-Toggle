using VRC.SDKBase;

namespace CorneliusCornbread.CrawlToggle
{
    public static class CornUtils
    {
        public static VRCPlayer GetLocalPlayer()
        {
            return VRCPlayer.field_Internal_Static_VRCPlayer_0;
        }
        
        public static VRCPlayerApi GetLocalPlayerApi()
        {
            return GetLocalPlayer().prop_VRCPlayerApi_0;
        }
    }
}