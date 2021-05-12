using UnityEngine;

namespace SimpleAR.MouseEmulation.Scripts
{
    [RequireComponent(typeof(MouseInputManager))]
    public class MouseTestDetector : HandDetector
    {
        protected new void Start()
        {
            base.Start();
            OnHandDetected.Invoke();
        }

        protected override void InitInstance()
        {
            MouseInputManager.OnEventOccured += ProceedOutput;
        }

        protected override void ProceedOutput()
        {
            var width = MouseInputManager.Instance.width;
            var height = MouseInputManager.Instance.height;

            Vector3 cursor = MouseInputManager.Instance.cursorPosition;
            cursor.z = MouseInputManager.Instance.depth;

            var cursorWorld = Camera.ScreenToWorldPoint(cursor);

            var boundingLeftTop = new Vector2(cursorWorld.x - width / 2f,
                cursorWorld.y + height / 2f);

            var points = new HandPoints {PalmCentre = cursorWorld};

            var action = MouseInputManager.Instance.action;
            var continuous = MouseInputManager.Instance.continuous;

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