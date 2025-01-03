using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearScene : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemChannel;

    public void Clear()
    {
        FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
        fadeEvt.isFadeIn = true;

        _systemChannel.AddListener<FadeComplete>(HandleFadeComplete);
        _systemChannel.RaiseEvent(fadeEvt);
    }

    private void HandleFadeComplete(FadeComplete obj)
    {
        _systemChannel.RemoveListener<FadeComplete>(HandleFadeComplete);
        SceneManager.LoadScene("EndingScene");
    }
}
