using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScene : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemChannel;

    private void Awake()
    {
        SoundManager.Instance.PlayBGM("GameOverBgm");
    }

    public void Restart()
    {
        FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
        fadeEvt.isFadeIn = true;

        _systemChannel.AddListener<FadeComplete>(HandleFadeComplete);
        _systemChannel.RaiseEvent(fadeEvt);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    private void HandleFadeComplete(FadeComplete obj)
    {
        _systemChannel.RemoveListener<FadeComplete>(HandleFadeComplete);
        
        if(StageSaveData.Instance != null)
            StageSaveData.Instance.isReset = true;
        if(HeartDataManager.instance != null)
            HeartDataManager.instance.Reset();
        
        SceneManager.LoadScene("StageSelectScene");
    }
}
