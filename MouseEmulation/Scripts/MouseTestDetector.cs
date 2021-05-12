using UnityEngine;

namespace SimpleAR.MouseEmulation.Scripts
{
    
    [RequireComponent(typeof(MouseInputManager))]
    public class MouseTestDetector : HandDetector
    {
        protected override void InitInstance()
        {
            MouseInputManager.OnEventOccured += ProceedOutput;
        }
        
        protected new void Start()
        {
            base.Start();
            OnHandDetected.Invoke();
        }

        protected override void ProceedOutput()
        {
            var width = MouseInputManager.Instance.Width;
            var height = MouseInputManager.Instance.Height;

            Vector3 cursor = MouseInputManager.Instance.CursorPosition;
            cursor.z = MouseInputManager.Instance.Depth;

            Vector3 cursorWorld = Camera.ScreenToWorldPoint(cursor);
            
            Vector2 boundingLeftTop = new Vector2(cursorWorld.x - width / 2f,
                cursorWorld.y + height / 2f);

            HandPoints points = new HandPoints {PalmCentre = cursorWorld};

            GestureAction action = MouseInputManager.Instance.Action;
            GestureContinuous continuous = MouseInputManager.Instance.Continuous;

            HandInfos.Insert(new HandInfo(
                new BoundingBox(boundingLeftTop, width, height),
                points,
                action,
                continuous));

            InvokeAction(action);
            InvokeContinuous(continuous);
        }
    }
}