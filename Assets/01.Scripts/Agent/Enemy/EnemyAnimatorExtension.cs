using System.Collections;
using System.Collections.Generic;
using Crogen.CrogenPooling;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimatorExtension : MonoBehaviour
{
    private Enemy enemy;
    [SerializeField] private UnityEvent attackEvent;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void PlayAttackAnim()
    {
        enemy.DamageCaster2D.CastDamage((int)enemy.statSO.damage);
        attackEvent?.Invoke();
    }

    public void PlayOnDie()
    {
        Destroy(enemy.gameObject);
    }

    public void TriggerSkillEffect()
    {
        gameObject.Pop(enemy.AttackEffectPoolType, transform);
    }
}
