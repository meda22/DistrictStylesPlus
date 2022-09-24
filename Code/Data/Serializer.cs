using System;
using System.IO;
using ColossalFramework.IO;
using DistrictStylesPlus.Code.Managers;
using DistrictStylesPlus.Code.Utils;
using ICities;

namespace DistrictStylesPlus.Code.Data
{
    public class Serializer : SerializableDataExtensionBase
    {
        
        // Current data version
        private const uint DataVersion = 0;

        // Unique data ID
        private const string DataId = "DistrictStylesPlusMod";

        // flattened data to save
        private static TransientDistrictStyleContainer[] _transientDistrictStyles;

        public override void OnCreated(ISerializableData serializableData)
        {
            base.OnCreated(serializableData);

            _transientDistrictStyles = new TransientDistrictStyleContainer[DSPTransientStyleManager.MaxDistrictCount];
        }

        public override void OnSaveData()
        {
            base.OnSaveData();
            
            Logging.InfoLog("Saving DSP data...");

            for (var i = 0; i < _transientDistrictStyles.Length; i++)
            {
                var data = DSPTransientStyleManager.GetStylesToSave((byte) i);
                
                if (data == null) continue; // no data for given district
                
                var transientDistrictStyle = new TransientDistrictStyleContainer();
                transientDistrictStyle.StyleFullNames = data;
                _transientDistrictStyles[i] = transientDistrictStyle;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                DataSerializer.SerializeArray(stream, DataSerializer.Mode.Memory, DataVersion, _transientDistrictStyles);
                serializableDataManager.SaveData(DataId, stream.ToArray());
                
                Logging.InfoLog("saved " + stream.Length + " B.");
            }
            
        }

        public override void OnLoadData()
        {
            base.OnLoadData();

            try
            {
                // read byte data from save game
                var byteData = serializableDataManager.LoadData(DataId);
                
                // check if anything to read
                if (byteData != null && byteData.Length > 0)
                {
                    using (MemoryStream stream = new MemoryStream(byteData))
                    {
                        _transientDistrictStyles =
                            DataSerializer.DeserializeArray<TransientDistrictStyleContainer>(stream,
                                DataSerializer.Mode.Memory);
                    }
                }
                else
                {
                    Logging.InfoLog("DSP does not have anything to read from save game.");
                }
            }
            catch (Exception e)
            {
                Logging.LogException(e, "DistrictStylePlus data failed to be loaded.");
            }
        }

        internal static TransientDistrictStyleContainer[] GetSavedData()
        {
            return _transientDistrictStyles;
        }

    }
}