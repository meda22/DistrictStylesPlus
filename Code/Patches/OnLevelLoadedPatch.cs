using DistrictStylesPlus.Code.GUI;
using DistrictStylesPlus.Code.Utils;
using HarmonyLib;

namespace DistrictStylesPlus.Code.Patches
{
    /// <summary>
    /// Harmony Postfix patch for OnLevelLoaded.  This enables us to perform setup tasks after all loading has been
    /// completed.
    /// </summary>
    // [HarmonyPatch(typeof(LoadingWrapper))]
    // [HarmonyPatch("OnLevelLoaded")]
    // public static class OnLevelLoadedPatch
    // {
    //     
    //     /// <summary>
    //     /// Harmony postfix to perform actions require after the level has loaded.
    //     /// </summary>
    //     public static void Postfix()
    //     {
    //         // Don't do anything if mod hasn't activated for whatever reason (mod conflict, harmony error, something else).
    //         if (!Loading.IsModEnabled)
    //         {
    //             return;
    //         }
    //         
    //         DistrictStyleEditorPanel.Initialize();
    //         MainButtonPanel.Initialize();
    //
    //         Logging.InfoLog("loading complete");
    //     }
    //     
    // }
}