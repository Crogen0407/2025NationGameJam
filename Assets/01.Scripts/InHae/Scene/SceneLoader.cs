using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemChannel;

    private void Start()
    {
        FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
        fadeEvt.isFadeIn = false;
        _systemChannel.RaiseEvent(fadeEvt);
    }
}
