using CitiesHarmony.API;
using DistrictStylesPlus.Code.Patches;
using ICities;

namespace DistrictStylesPlus.Code
{
    public class DistrictStylesPlusMod : IUserMod
    {
        internal static string modName => "District Styles Plus";
        internal static string version => baseVersion + " " + versionNote;
        internal static string versionNote => "alpha wip";
        private static string baseVersion => "0.0.1";

        public string Name => modName + " " + version;
        public string Description => "Enhances district styles functionality and extends DS configuration possibilities";

        /// <summary>
        /// Called by the game when the mod is enabled.
        /// </summary>
        public void OnEnabled()
        {
            // TODO: load mod settings here if any exists
            
            // Apply Harmony patches via Cities Harmony.
            // Called here instead of OnCreated to allow the auto-downloader to do its work prior to launch.
            HarmonyHelper.DoOnHarmonyReady(Patcher.PatchAll);
            
            // TODO: add attaching of options panel hook if any options panel ever exists
        }
        
        /// <summary>
        /// Called by the game when the mod is disabled.
        /// </summary>
        public void OnDisabled()
        {
            // Unapply Harmony patches via Cities Harmony.
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Patcher.UnpatchAll();
            }
        }

        public void OnSettingsUI(UIHelperBase uiHelperBase)
        {
            
        }
        
    }
}