using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthContainer : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2 _originPosition;
    public DefaultHealthSystem _playerHealthSystem;
    [SerializeField] private Slider _playerHealthSlider;
    
    private void Awake()
    {
        _rectTransform = transform as RectTransform;
        _originPosition = _rectTransform.anchoredPosition;
        
    }

    private void Start()
    {
        _playerHealthSystem = FindFirstObjectByType<Player>().GetComponent<DefaultHealthSystem>();
        _playerHealthSystem.hpDownEvent.AddListener(HandleTakeDamage);
        _playerHealthSystem.hpChangeEvent.AddListener(HandleChangeHealth);
    }

    private void HandleChangeHealth()
    {
        _playerHealthSlider.value = _playerHealthSystem.Hp / _playerHealthSystem.maxHp;
    }

    private void HandleTakeDamage()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_rectTransform.DOShakeAnchorPos(0.1f, 10));
        seq.Append(_rectTransform.DOAnchorPos(_originPosition, 0.1f));
        seq.SetUpdate(false);
    }
}
