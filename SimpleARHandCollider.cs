using System;
using UnityEngine;

namespace SimpleAR
{
    public class SimpleARHandCollider : MonoBehaviour
    {
        public static String ColliderTag = "Player";
        public static SimpleARHandCollider Instance { get; set; }

        public Vector3 handPosition;

        public bool isDetected = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            //    HandDetector.OnHandDetected += () =>
                {
                    isDetected = true;
                    gameObject.SetActive(true);
                };
          //      HandDetector.OnHandLost += () =>
          //      {
          //          isDetected = false;
          //          gameObject.SetActive(false);
          //      };   
            }
            else
                gameObject.SetActive(false);
        }

        private void Start()
        {
            gameObject.tag = ColliderTag;
        }

        private void Update()
        {
            if(!isDetected)
                return;
            var centre = HandDetector.Instance.HandInfos[0].HandPoints.PalmCentre;
            transform.position = centre;
            handPosition = centre;
        }
    }
}
