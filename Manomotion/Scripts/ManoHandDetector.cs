namespace SimpleAR.Manomotion.Scripts
{
    public class ManoHandDetector : HandDetector
    {
        protected new void Start()
        {
            base.Start();
            OnHandDetected.Invoke();
        }

        public void LateUpdate()
        {
            if (!IsActive)
                ManomotionManager.Instance.StopProcessing();
        }

        protected override void InitInstance()
        {
            ManomotionManager.OnManoMotionFrameProcessed += ProceedOutput;
        }

        protected override void ProceedOutput()
        {
            var manoInfo = ManomotionManager.Instance.Hand_infos[0];

            var mano = ConvertHandInfo(manoInfo);
            HandInfos.Insert(mano);

            InvokeAction(mano.Action);
            InvokeContinuous(mano.Continuous);
        }

        private HandInfo ConvertHandInfo(HandInfoUnity infoUnity)
        {
            var info = infoUnity.hand_info;
            var (box, fingers) = ConvertTrackingInfo(info.tracking_info);
            var (actionGesture, contGesture) = ConvertGestureInfo(info.gesture_info);
            return new HandInfo(box, fingers, actionGesture, contGesture, info.tracking_info.depth_estimation);
        }

        private (BoundingBox, HandPoints) ConvertTrackingInfo(TrackingInfo tracking)
        {
            var box = tracking.bounding_box;
            box.top_left = Camera.ViewportToWorldPoint(box.top_left);
            box.top_left.z = tracking.depth_estimation;
            var points = new HandPoints();
            var palm = tracking.palm_center;
            palm.z = tracking.depth_estimation;
            points.PalmCentre = Camera.ViewportToWorldPoint(palm);
            return (new BoundingBox(box.top_left, box.width, box.height), points);
        }

        private (GestureAction, GestureContinuous) ConvertGestureInfo(GestureInfo gesture)
        {
            GestureAction gestureAction;
            switch (gesture.mano_gesture_trigger)
            {
                case ManoGestureTrigger.CLICK:
                    gestureAction = GestureAction.ClickAction;
                    break;
                case ManoGestureTrigger.PICK:
                case ManoGestureTrigger.GRAB_GESTURE:
                    gestureAction = GestureAction.GrabAction;
                    break;
                case ManoGestureTrigger.DROP:
                case ManoGestureTrigger.RELEASE_GESTURE:
                    gestureAction = GestureAction.ReleaseAction;
                    break;
                default:
                    gestureAction = GestureAction.NoAction;
                    break;
            }

            GestureContinuous gestureContinuous;
            switch (gesture.mano_gesture_continuous)
            {
                case ManoGestureContinuous.HOLD_GESTURE:
                    gestureContinuous = GestureContinuous.Hold;
                    break;
                case ManoGestureContinuous.POINTER_GESTURE:
                    gestureContinuous = GestureContinuous.Point;
                    break;
                default:
                    gestureContinuous = GestureContinuous.NoContinuous;
                    break;
            }

            return (gestureAction, gestureContinuous);
        }
    }
}