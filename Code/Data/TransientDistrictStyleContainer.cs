using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using ColossalFramework.IO;
using DistrictStylesPlus.Code.Utils;

namespace DistrictStylesPlus.Code.Data
{
    /// <summary>
    /// Serialisation / DeSerialisation of district styles selections to save game file.
    /// </summary>
    public class TransientDistrictStyleContainer : IDataContainer
    {

        public HashSet<string> StyleFullNames;
        
        public void Serialize(DataSerializer s)
        {
            Logging.InfoLog("Write DistrictStylesPlus data.");
            s.WriteUniqueStringArray(StyleFullNames.ToArray());
        }

        public void Deserialize(DataSerializer s)
        {
            Logging.InfoLog("Load DistrictStylesPlus data.");
            StyleFullNames = new HashSet<string>(s.ReadUniqueStringArray());
        }

        public void AfterDeserialize(DataSerializer s)
        {
            Logging.InfoLog("Validate DistrictStylesPlus data.");

            if (!DistrictManager.exists)
            {
                Logging.ErrorLog("Load from save game problem. District Manager does not exist.");
                return;
            }
            
            Logging.DebugLog($"Check if all styles exists: {string.Join(", ", StyleFullNames.ToArray())}");

            if (StyleFullNames.Count <= 0) return; // no styles mentioned, nothing to validate
            
            var validatedStylesNames = Singleton<DistrictManager>.instance.m_Styles
                .Where(style => StyleFullNames.Contains(style.FullName))
                .Select(style => style.FullName)
                .ToArray();
                
            StyleFullNames = new HashSet<string>(validatedStylesNames);
        }
    }
}