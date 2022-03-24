using MelonLoader;

namespace CorneliusCornbread.CrawlToggle
{
    public static class CTPreferences
    {
        public static MelonPreferences_Category ModCategory { get; private set; }

        public static MelonPreferences_Entry<bool> CrawlTogglePref { get; private set; }
        public static MelonPreferences_Entry<float> CrawlSpeedPref { get; private set; }

        public static MelonPreferences_Entry<bool> WalkTogglePref { get; private set; }
        public static MelonPreferences_Entry<float> WalkSpeedPref { get; private set; }
        
        public static void Init()
        {
            ModCategory = MelonPreferences.CreateCategory(CrawlSpeedToggle.ModCategoryId, ModBuildInfo.Name);
            
            CrawlTogglePref = ModCategory.CreateEntry("Crawl Toggle", false);
            WalkTogglePref = ModCategory.CreateEntry("Walk Toggle", false);

            CrawlSpeedPref = ModCategory.CreateEntry("Crawl Speed", 0.5f);
            WalkSpeedPref = ModCategory.CreateEntry("Walk Speed", 2f);
        }
    }
}