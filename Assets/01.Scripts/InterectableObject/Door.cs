using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Door : InteractableObject
{
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
        transform.localScale = new Vector3(1, 0, 0);
    }

    private void Start()
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(2f);
        seq.Append(transform.DOScaleY(1, 1.8f).SetEase(Ease.OutCirc));
        seq.AppendCallback(() => _collider.enabled = true);
    }
}
