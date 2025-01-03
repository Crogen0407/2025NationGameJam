using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScene : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemChannel;


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
        StageSaveData.Instance.isReset = true;
        HeartDataManager.instance.Reset();
        
        SceneManager.LoadScene("StageSelectScene");
    }
}
