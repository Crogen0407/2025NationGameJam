using Crogen.CrogenPooling;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raser : MonoBehaviour
{
    private DamageCaster2D damageCaster;
    public float damage;
    private float raserValue = 0.5f;
    [SerializeField] private EffectPoolType raserEffect;
    [SerializeField] private Transform effectPos;

    private void Awake()
    {
        damageCaster = GetComponentInChildren<DamageCaster2D>();
    }

    private void Start()
    {
        StartCoroutine(TickDamaage());
        StartCoroutine(Co_DestoryRaser());
        gameObject.Pop(raserEffect, effectPos);
    }

    void Update()
    {
        transform.DORotate(new Vector3(0, 0, 360), 15, RotateMode.FastBeyond360);
    }

    IEnumerator TickDamaage()
    {
        while (true)
        {
            Debug.Log("레이자피해");
            damageCaster.CastDamage((int)(damage * raserValue));
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator Co_DestoryRaser()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
