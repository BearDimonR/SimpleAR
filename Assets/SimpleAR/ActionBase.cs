using UnityEngine;

namespace SimpleAR
{
    public abstract class ActionBase : MonoBehaviour
    {
        protected void Awake()
        {
            HandDetector.OnHandClicked += OnHandClicked;
            HandDetector.OnHandGrabbed += OnHandGrabbed;
            HandDetector.OnHandReleased += OnHandReleased;
            
            HandDetector.OnHandHold += OnHandHold;
            HandDetector.OnHandPoint += OnHandPoint;
            
            HandDetector.OnNoHandAction += OnNoHandAction;
            HandDetector.OnNoHandContAction += OnNoHandContAction;
            
            HandDetector.OnHandDetected += OnHandDetected;
            HandDetector.OnHandLost += OnHandLost;
        }
        
        protected virtual void OnHandLost()
        {
        }

        protected virtual void OnHandDetected()
        {
        }
        
        protected virtual void OnNoHandContAction()
        {
        }

        protected virtual void OnNoHandAction()
        {
        }

        protected virtual void OnHandPoint()
        {
        }

        protected virtual void OnHandHold()
        {
        }

        protected virtual void OnHandReleased()
        {
        }

        protected virtual void OnHandGrabbed()
        {
        }

        protected virtual void OnHandClicked()
        {
        }
    }
}