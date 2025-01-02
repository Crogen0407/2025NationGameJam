using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemChannel;
    [SerializeField] private Image _fadeImage;

    private void Awake()
    {
        _systemChannel.AddListener<FadeScreenEvent>(HandleFadeScreen);
    }

    private void OnDestroy()
    {
        _systemChannel.RemoveListener<FadeScreenEvent>(HandleFadeScreen);
    }

    private void HandleFadeScreen(FadeScreenEvent evt)
    {
        float fadeValue = evt.isFadeIn ? 1f : 0f;
        float startValue = evt.isFadeIn ? 0f : 1f;

        Sequence sequence = DOTween.Sequence();
        _fadeImage.color = new Color(0, 0, 0, startValue);
        
        sequence.Append(_fadeImage.DOFade(fadeValue, 0.8f).OnComplete(() =>
        {
            _systemChannel.RaiseEvent(SystemEvents.FadeComplete);
        }));
    }
}
