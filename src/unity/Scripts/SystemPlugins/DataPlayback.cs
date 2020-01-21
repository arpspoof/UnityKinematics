using UnityEngine;
using System.Collections.Generic;
using System.IO;
using static UnityKinematics.SerializableObjects;

namespace UnityKinematics
{
    public class DataPlayback : MonoBehaviour
    {
        public string pathToFile;
        public bool feedDataAtStartTime = true;

        private static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        public void FeedData()
        {
            if (!isActiveAndEnabled) return;
            Debug.Log("Feed data from file");

            var cmdBuffer = UnityServerAPI.RPCGetCommandBuffer();
            var frameBuffer = UnityServerAPI.RPCGetFrameBuffer();

            var dataList = ReadFromBinaryFile<List<SerializableData>>(pathToFile);
            foreach (var data in dataList)
            {
                if (data.GetType() == typeof(SerializableCommand)) 
                    cmdBuffer.Write((SerializableCommand)data);
                if (data.GetType() == typeof(SerializableFrameState)) 
                    frameBuffer.Write((SerializableFrameState)data);
            }
        }

        void Awake()
        {
            KinematicsServerEvents.OnServerInitialize += FeedData;
        }
    }
}
