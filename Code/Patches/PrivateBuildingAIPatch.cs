using System.Linq;
using ColossalFramework;
using DistrictStylesPlus.Code.Managers;
using HarmonyLib;
using UnityEngine;

namespace DistrictStylesPlus.Code.Patches
{
    [HarmonyPatch]
    public static class GetUpgradeInfoPatch
    {

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PrivateBuildingAI), "GetUpgradeInfo")]
        public static void GetUpgradedInfoPostfix(ref BuildingInfo __result, ushort buildingID, ref Building data)
        {
            // option to allow upgrade with same building appearance is disabled => do nothing
            if (!ModSettings.allowUpgradeSameAppearance) return;
            
            // there is suitable building to upgrade to => do nothing
            if (__result != null) return;

            lock (DistrictStylesPlusManager.KeepSameAppearanceBuildingIds)
            {
                DistrictStylesPlusManager.KeepSameAppearanceBuildingIds.Add(buildingID);
            }
            
            __result = data.Info;
        }
    }

    [HarmonyPatch]
    public static class StartUpgradingPatch
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PrivateBuildingAI), "StartUpgrading")]
        public static bool StartUpgradingPrefix(ushort buildingID, ref Building buildingData, PrivateBuildingAI __instance)
        {
            
            // option to allow upgrade with same building appearance is disabled => do nothing
            if (!ModSettings.allowUpgradeSameAppearance) return true;

            bool keepAppearance;

            // check if building is marked to keep appearance with upgrade
            lock (DistrictStylesPlusManager.KeepSameAppearanceBuildingIds)
            {
                keepAppearance = DistrictStylesPlusManager.KeepSameAppearanceBuildingIds.Contains(buildingID);
                if (keepAppearance)
                {
                    DistrictStylesPlusManager.KeepSameAppearanceBuildingIds.Remove(buildingID);
                }
            }

            // upgrade building but do not change prefab
            if (keepAppearance)
            {
                BuildingManager buildingManager = Singleton<BuildingManager>.instance;
                EffectInfo levelUpEffect = buildingManager.m_properties.m_levelupEffect;

                if (levelUpEffect != null)
                {
                    InstanceID instanceID = default(InstanceID);
                    instanceID.Building = buildingID;
                    Vector3 meshPosition;
                    Quaternion meshRotation;
                    buildingData.CalculateMeshPosition(out meshPosition, out meshRotation);
                    Matrix4x4 matrix4x = Matrix4x4.TRS(meshPosition, meshRotation, Vector3.one);
                    EffectInfo.SpawnArea spawnArea = new EffectInfo.SpawnArea(matrix4x, __instance.m_info.m_lodMeshData);
                    Singleton<EffectManager>.instance.DispatchEffect(levelUpEffect, instanceID, spawnArea, Vector3.zero, 0f, 1f, buildingManager.m_audioGroup);
                }
                
                Vector3 position = buildingData.m_position;
                position.y += __instance.m_info.m_size.y;
                Singleton<NotificationManager>.instance.AddEvent(NotificationEvent.Type.LevelUp, position, 1f);
                
                Building.Flags flags = buildingData.m_flags;
                flags &= ~Building.Flags.LevelUpEducation;
                flags &= ~Building.Flags.LevelUpLandValue;
                buildingData.m_flags = flags;
                buildingData.m_level += 1;
                
                __instance.BuildingUpgraded(buildingID, ref buildingData);
                
                object[] parameters = new object[2];
                parameters[0] = buildingID;
                parameters[1] = buildingData;
                AccessTools.Method(__instance.GetType(), "ManualActivation").Invoke(__instance, parameters);

                return false;
            }

            return true;
        }
        
    } 
}