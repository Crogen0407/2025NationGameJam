using UnityEngine;
using UnityEngine.SceneManagement;

public class StageInGameManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemEventChannel;
    [SerializeField] private string _nextSceneName;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
            StageClear();
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
            fadeEvt.isFadeIn = true;

            StageSaveData.Instance.isReset = true;
            _systemEventChannel.AddListener<FadeComplete>(HandleFadeComplete);
            _systemEventChannel.RaiseEvent(fadeEvt);
        }
    }

    public void StageClear()
    {
        FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
        fadeEvt.isFadeIn = true;

        StageSaveData.Instance.blockDictionary[StageSaveData.Instance.currentKey].isClear = true;
        _systemEventChannel.AddListener<FadeComplete>(HandleFadeComplete);
        _systemEventChannel.RaiseEvent(fadeEvt);
    }

    private void HandleFadeComplete(FadeComplete obj)
    {
        _systemEventChannel.RemoveListener<FadeComplete>(HandleFadeComplete);
        SceneManager.LoadScene(_nextSceneName);
    }
}
