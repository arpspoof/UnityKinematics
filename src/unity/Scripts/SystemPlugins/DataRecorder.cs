using UnityEngine;
using System.Collections.Generic;
using System.IO;
using static UnityKinematics.SerializableObjects;

namespace UnityKinematics
{
    public class DataRecorder : MonoBehaviour
    {
        public string pathToFile;
        public bool autoStart = true;
        public bool autoStopAndWrite = true;

        private List<SerializableData> data;
        private int frameCount;

        private static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public void StartRecording()
        {
            Debug.Log("Start data recording");
            frameCount = 0;
            data = new List<SerializableData>();
            KinematicsServerEvents.OnCommand += RecordCommand;
            KinematicsServerEvents.OnSystemCommand += RecordCommand;
            KinematicsServerEvents.OnAfterNewFrame += RecordFrame;
        }

        public void StopRecordingAndWriteToFile()
        {
            Debug.Log("Stop data recording and write to file");
            KinematicsServerEvents.OnCommand -= RecordCommand;
            KinematicsServerEvents.OnSystemCommand -= RecordCommand;
            KinematicsServerEvents.OnAfterNewFrame -= RecordFrame;
            WriteToBinaryFile(pathToFile, data);
        }

        void Start()
        {
            if (autoStart && isActiveAndEnabled) 
                StartRecording();
        }

        void OnApplicationQuit()
        {
            if (autoStopAndWrite && isActiveAndEnabled) 
                StopRecordingAndWriteToFile();
        }

        private void RecordFrame(FrameState frame)
        {
            frameCount++;
            data.Add(new SerializableFrameState(frame));
        }

        private void RecordCommand(Command cmd)
        {
            data.Add(new SerializableCommand(cmd, frameCount));
        }
    }
}
