using UnityEngine;

namespace SimpleAR
{
    public struct HandInfo
    {
        public BoundingBox BoundingBox;
        public HandPoints HandPoints;
        public GestureAction Action;
        public GestureContinuous Continuous;
        public float Depth;

        public HandInfo(BoundingBox boundingBox, HandPoints handPoints, 
            GestureAction action, GestureContinuous continuous, float depth = 0)
        {
            BoundingBox = boundingBox;
            HandPoints = handPoints;
            Action = action;
            Continuous = continuous;
            Depth = depth;
        }
    }

    public enum GestureContinuous
    {
        NoContinuous = 0,
        Hold = 1,
        Point = 2
    }

    public enum GestureAction
    {
        NoAction = 0,
        ClickAction = 1,
        GrabAction = 2,
        ReleaseAction = 3
    }

    public struct HandPoints
    {
        public Vector3 PalmCentre;
        public Vector3 Ring;
        public Vector3 Thumb;
        public Vector3 Middle;
        public Vector3 Index;
        public Vector3 Pinky;

        public HandPoints(Vector3 palmCentre, Vector3 ring, Vector3 thumb, Vector3 middle, Vector3 index, Vector3 pinky)
        {
            PalmCentre = palmCentre;
            Ring = ring;
            Thumb = thumb;
            Middle = middle;
            Index = index;
            Pinky = pinky;
        }
    }

    public struct BoundingBox
    {
        public Vector3 TopLeftPoint;
        public float Width;
        public float Height;

        public BoundingBox(Vector3 topLeftPoint, float width, float height)
        {
            TopLeftPoint = topLeftPoint;
            Width = width;
            Height = height;
        }
    }
}