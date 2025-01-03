using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeartPart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SpriteRenderer _spriteRenderer;
    private Vector3 _defaultScale;

    public static bool isClick;
    
    private void Awake()
    {
        _defaultScale = transform.localScale;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        isClick = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //처음 시작했을 때
        if (StageSaveData.Instance.currentKey == null || isClick)
            return;
        //이미 색칠이 됐을 때
        if(_spriteRenderer.color != Color.white)
            return;

        _spriteRenderer.sortingOrder = 1;
        Vector3 targetScale = new Vector3(_defaultScale.x + 0.25f, _defaultScale.y + 0.25f);
        transform.DOScale(targetScale, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (StageSaveData.Instance.currentKey == null || isClick)
            return;
        if(_spriteRenderer.color != Color.white)
            return;
        SmallSize();
    }

    public void SmallSize()
    {
        _spriteRenderer.sortingOrder = 0;
        Vector3 targetScale = new Vector3(_defaultScale.x, _defaultScale.y);
        transform.DOScale(targetScale, 0.1f);
    }
}
