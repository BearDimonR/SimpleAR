using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace SimpleAR.Examples.Chess.Scripts
{
    public class PlaceOnTouch : MonoBehaviour
    {
        [SerializeField] private ARRaycastManager mRaycastManager;

        [SerializeField] private GameObject chessBoard;

        private ARPlaneManager _arPlaneManager;
        private Camera _cameraMain;

        private void Start()
        {
            _arPlaneManager = gameObject.GetComponent<ARPlaneManager>();
            _cameraMain = Camera.main;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                var worldScreen = _cameraMain.ViewportToWorldPoint(touch.position);

                var ray = new Ray(worldScreen, _cameraMain.transform.forward);

                var hitResults = new List<ARRaycastHit>();

                if (mRaycastManager.Raycast(touch.position, hitResults, TrackableType.Planes))
                {
                    Instantiate(chessBoard, hitResults[0].pose.position, gameObject.transform.rotation);

                    _arPlaneManager.enabled = false;
                    foreach (var plane in _arPlaneManager.trackables)
                        plane.gameObject.SetActive(false);
                    enabled = false;
                }
            }
        }
    }
}