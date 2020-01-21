using UnityEngine;

namespace UnityKinematics
{
    [RequireComponent(typeof(LineRenderer))]
    public class TrajectoryMarker : MonoBehaviour
    {
        [Range(1, 10000)]
        public int framesPerMarker = 1;
        public string trackedObjectName = "root";

        private int frameCounter = 0;

        void Reset()
        {
            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
        }

        void Start()
        {
            KinematicsServerEvents.OnAfterNewFrame += OnNewFrame;
        }

        public void Clear()
        {
            Reset();
        }

        private void OnNewFrame(FrameState frame)
        {
            if (frameCounter++ % framesPerMarker != 0) return;

            var obj = GameObject.Find(trackedObjectName);
            if (!obj)
            {
                Debug.LogWarning($"TrajectoryMarker: tracked object with name {trackedObjectName} does not exist");
                return;
            }

            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, obj.transform.position);
        }
    }
}
