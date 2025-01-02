using UnityEngine;

public static class CameraEvents
{
    public static CameraZoomInEvent CameraZoomInEvent = new CameraZoomInEvent();
}

public class CameraZoomInEvent : GameEvent
{
    public Vector3 targetPos;
    public float lensSize;
    public float moveTime;
    public float zoomInTime;
}