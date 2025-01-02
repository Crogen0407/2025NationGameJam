using Crogen.AgentFSM.Boss;
using Crogen.HealthSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class BossInfoContainer : MonoBehaviour
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
        _healthSlider.value = (_bossHealthSystem.Hp / _bossHealthSystem.maxHp);
        _rectTransform.DOShakeAnchorPos(0.1f, 1)
            .OnComplete(()=>_rectTransform.DOAnchorPos(new Vector3(0, -27), 0.1f));
    }
}
