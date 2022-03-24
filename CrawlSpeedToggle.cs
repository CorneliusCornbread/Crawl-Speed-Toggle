using MelonLoader;

namespace CorneliusCornbread.CrawlToggle
{
    public class CrawlSpeedToggle : MelonMod
    {
        public const string ModCategoryId = "CC_CrawlToggle";

        /// <summary>
        /// The percentage of speed based on the world's run speed.
        /// </summary>
        private const float _speedMult = .1f;
        
        public static MelonLogger.Instance Logger { get; private set; }
        
        public override void OnApplicationStart()
        {
            Logger = LoggerInstance;
            
            Logger.Msg("---Crawl Speed Toggle Init---");
            
            CTPreferences.Init();
            PlayerSpeed.Init();
        }

        private static bool GetCrawlToggleState()
        {
            return CTPreferences.CrawlTogglePref.Value;
        }
        
        private static bool GetWalkToggleState()
        {
            return CTPreferences.WalkTogglePref.Value;
        }
    }
}