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


    private void Start()
    {
        StartCoroutine(TickDamaage());
        StartCoroutine(Co_DestoryRaser());
    }

    private void OnEnable()
    {

        SimplePoolingObject raser = gameObject.Pop(raserEffect, effectPos) as SimplePoolingObject;
        raser.transform.localRotation = Quaternion.Euler(0, 0, 90);
        damageCaster = GetComponentInChildren<DamageCaster2D>();
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
            yield return new WaitForSeconds(0.2f);
            CameraShakeController.Shake(0.1f, 2f);
        }
    }

    IEnumerator Co_DestoryRaser()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
