using UnityEngine;

namespace SimpleAR
{
    public class SimpleARHandCollider : MonoBehaviour
    {
        public static string ColliderTag = "Player";

        public Vector3 handPosition;

        public bool isDetected;
        public static SimpleARHandCollider Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                HandDetector.OnHandDetected += () =>
                {
                    Debug.Log("OnHandDetected");
                    isDetected = true;
                    gameObject.SetActive(true);
                };
                HandDetector.OnHandLost += () =>
                {
                    Debug.Log("OnHandLost");
                    isDetected = false;
                    gameObject.SetActive(false);
                };
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            gameObject.tag = ColliderTag;
        }

        private void Update()
        {
            if (!isDetected)
                return;
            var centre = HandDetector.Instance.HandInfos[0].HandPoints.PalmCentre;
            transform.position = centre;
            handPosition = centre;
        }
    }
}