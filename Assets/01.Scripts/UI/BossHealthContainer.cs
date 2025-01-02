using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthContainer : MonoBehaviour
{
    private Boss _boss;
    private DefaultHealthSystem _bossHealthSystem;
    private RectTransform _rectTransform;
    
    [SerializeField] private Slider _healthSlider;
    
    private void Awake()
    {
        _rectTransform = transform as RectTransform;
    }

    private void Start()
    {
        _boss = FindFirstObjectByType<Boss>();

        if (_boss != null)
        {
            _bossHealthSystem = _boss.GetComponent<DefaultHealthSystem>();
            _bossHealthSystem.hpChangeEvent.AddListener(HandleBossHpChanged);
            _rectTransform.DOAnchorPosY(-27, 0.5f).SetEase(Ease.OutCirc).SetUpdate(false);
        }
    }

    private void HandleBossHpChanged()
    {
        _rectTransform.DOShakeAnchorPos(0.1f, 10)
            .OnComplete(()=>_rectTransform.DOAnchorPos(new Vector3(0, -27), 0.1f).SetUpdate(false)).SetUpdate(false);
        _healthSlider.value = (_bossHealthSystem.Hp / _bossHealthSystem.maxHp);
    }
}
