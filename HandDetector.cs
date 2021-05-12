using System;
using UnityEngine;

namespace SimpleAR
{
    public abstract class HandDetector : MonoBehaviour
    {

        #region Singleton

        private static HandDetector _instance;

        public static HandDetector Instance => _instance;

        #endregion

        #region Events

        public static Action OnHandDetected;
        public static Action OnHandLost;
        public static Action OnNoHandAction;
        public static Action OnHandClicked;
        public static Action OnHandGrabbed;
        public static Action OnHandReleased; 
        public static Action OnHandHold;
        public static Action OnHandPoint;
        public static Action OnNoHandContAction;

        #endregion

        #region Variables

        protected static int HAND_HISTORY = 20;

        protected Camera Camera;
        
        public ShiftArray<HandInfo> HandInfos
        {
            get;
            protected set;
        }

        protected bool IsActive;

        #endregion

        #region Abstract Methods

        protected abstract void ProceedOutput();
        protected abstract void InitInstance();

        #endregion

        #region Methods

        public void StopDetection()
        {
            IsActive = false;
        }

        public void StartDetection()
        {
            IsActive = true;
        }

        #endregion

        #region UnityEvents

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                HandInfos = new ShiftArray<HandInfo>(HAND_HISTORY);
                IsActive = true;
                
                InitInstance();
            }
            else
                gameObject.SetActive(false);
        }

        protected void Start()
        {
            Camera = Camera.main;
        }

        #endregion

        #region InvokeMethods

        protected void InvokeContinuous(GestureContinuous gestureContinuous)
        {
            switch (gestureContinuous)
            {
                case GestureContinuous.Hold:
                    OnHandHold.Invoke();
                    break;
                case GestureContinuous.Point:
                    OnHandPoint.Invoke();
                    break;
                default:
                    OnNoHandContAction.Invoke();
                    break;
            }
        }
        
        protected void InvokeAction(GestureAction gestureAction)
        {
            switch (gestureAction)
            {
                case GestureAction.ClickAction:
                    OnHandClicked.Invoke();
                    break;
                case GestureAction.GrabAction:
                    OnHandGrabbed.Invoke();
                    break;
                case GestureAction.ReleaseAction:
                    OnHandReleased.Invoke();
                    break;
                default:
                    OnNoHandAction.Invoke();
                    break;
            }
        }

        #endregion
    }
}