using UnityEngine;

public static class SystemEvents
{
    public static readonly FadeScreenEvent FadeScreenEvent = new FadeScreenEvent();
    public static readonly FadeComplete FadeComplete = new FadeComplete();
}

public class FadeScreenEvent : GameEvent
{
    public bool isFadeIn;
}

public class FadeComplete : GameEvent
{
    
}
