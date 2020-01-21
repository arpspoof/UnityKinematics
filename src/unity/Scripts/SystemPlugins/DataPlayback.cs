using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static UnityKinematics.SerializableObjects;

namespace UnityKinematics
{
    public class DataPlayback : MonoBehaviour
    {
        public string pathToFile;

        private List<SerializableFrameState> frameList;
        private List<SerializableCommand> commandList;
        private int commandPointer;

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

            commandPointer = 0;

            var cmdBuffer = UnityServerAPI.RPCGetCommandBuffer();
            var frameBuffer = UnityServerAPI.RPCGetFrameBuffer();

            var dataList = ReadFromBinaryFile<List<SerializableData>>(pathToFile);
            frameList = (
                from d in dataList 
                where d.GetType() == typeof(SerializableFrameState)
                select d as SerializableFrameState
            ).ToList();
            commandList = (
                from d in dataList 
                where d.GetType() == typeof(SerializableCommand)
                select d as SerializableCommand
            ).ToList();

            frameList.ForEach(f => UnityServerAPI.RPCGetFrameBuffer().Write(f));
        }

        private void FeedCommands(int frameId)
        {
            if (!isActiveAndEnabled) return;
            int prevCommandPointer = commandPointer;
            while (commandList.Count > commandPointer && frameId == commandList[commandPointer].frameId)
            {
                UnityServerAPI.RPCGetCommandBuffer().Write(commandList[commandPointer]);
                commandPointer++;
            }
            if (commandPointer > prevCommandPointer)
            {
                KinematicsServer.instance.ExecutePendingCommands();
            }
        }

        void Awake()
        {
            KinematicsServerEvents.OnServerInitialize += FeedData;
            KinematicsServerEvents.OnBeforeNewFrame += FeedCommands;
        }
        
        void Start()
        {
        }
    }
}
