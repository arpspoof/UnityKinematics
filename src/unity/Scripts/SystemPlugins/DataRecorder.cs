using UnityEngine;
using System.Collections.Generic;
using System.IO;
using static UnityKinematics.SerializableObjects;

namespace UnityKinematics
{
    public class DataRecorder : MonoBehaviour
    {
        public string pathToFile;
        public bool append = false;
        public bool autoStart = true;
        public bool autoStopAndWrite = true;

        private List<SerializableData> data;

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
            if (!isActiveAndEnabled) return;
            Debug.Log("Start data recording");
            data = new List<SerializableData>();
            KinematicsServerEvents.OnCommand += RecordCommand;
            KinematicsServerEvents.OnSystemCommand += RecordCommand;
            KinematicsServerEvents.OnNewFrame += RecordFrame;
        }

        public void StopRecordingAndWriteToFile()
        {
            if (!isActiveAndEnabled) return;
            Debug.Log("Stop data recording and write to file");
            KinematicsServerEvents.OnCommand -= RecordCommand;
            KinematicsServerEvents.OnSystemCommand -= RecordCommand;
            KinematicsServerEvents.OnNewFrame -= RecordFrame;
            WriteToBinaryFile(pathToFile, data, append);
        }

        void Start()
        {
            if (autoStart) StartRecording();
        }

        void OnApplicationQuit()
        {
            if (autoStopAndWrite) StopRecordingAndWriteToFile();
        }

        private void RecordFrame(FrameState frame) => data.Add(new SerializableFrameState(frame));
        private void RecordCommand(Command cmd) => data.Add(new SerializableCommand(cmd));
    }
}
