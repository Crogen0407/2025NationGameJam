using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemEventChannel;
    [SerializeField] private string _nextSceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
            fadeEvt.isFadeIn = true;

            StageSaveData.blockDictionary[StageSaveData.currentKey].isClear = true;
            _systemEventChannel.AddListener<FadeComplete>(HandleFadeComplete);
            _systemEventChannel.RaiseEvent(fadeEvt);
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
            fadeEvt.isFadeIn = true;

            StageSaveData.isEnd = true;
            _systemEventChannel.AddListener<FadeComplete>(HandleFadeComplete);
            _systemEventChannel.RaiseEvent(fadeEvt);
        }
    }

    private void HandleFadeComplete(FadeComplete obj)
    {
        _systemEventChannel.RemoveListener<FadeComplete>(HandleFadeComplete);
        SceneManager.LoadScene(_nextSceneName);
    }
}
