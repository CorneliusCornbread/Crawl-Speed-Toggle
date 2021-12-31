using MelonLoader;

namespace CorneliusCornbread
{
    public class CrawlSpeedToggle : MelonMod
    {
        private const string ModCategoryId = "CC_CrawlToggle";
        private static MelonPreferences_Category _modCategory;
        private static MelonPreferences_Entry<bool> _togglePreference;

        public static MelonLogger.Instance Logger { get; private set; }

        public override void OnApplicationStart()
        {
            Logger = LoggerInstance;
            
            Logger.Msg("---Crawl Speed Toggle Init---");
            
            _modCategory = MelonPreferences.CreateCategory(ModCategoryId, ModBuildInfo.Name);
            _togglePreference = _modCategory.CreateEntry(CTModSetting.CrawlToggle.ToString(), false);
            
        }
    }
}