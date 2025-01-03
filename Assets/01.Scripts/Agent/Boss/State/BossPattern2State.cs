using Crogen.AgentFSM;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BossPattern2State : AgentState
{
    public BossPattern2State(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }

    private Rigidbody2D rb;

    public override void Enter()
    {
        base.Enter();
        rb = _agentBase.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        _agentBase.StartCoroutine(Co_RaserPattern());
        Debug.Log("2번 패턴 실행");
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
        base.Exit();
    }

    IEnumerator Co_RaserPattern()
    {
        Boss _boss = _agentBase as Boss;
        float originalGravity = rb.gravityScale;
        List<int> num = new List<int>();

        for (int i = 0; i < _boss.raserAttackTransforms.Count; i++)
        {
            int index = Random.Range(0, _boss.raserAttackTransforms.Count);

            while (num.Contains(index))
            {
                index = Random.Range(0, _boss.raserAttackTransforms.Count);
            }

            num.Add(index);
            _agentBase.transform.position = _boss.raserAttackTransforms[num[i]].position;
            rb.gravityScale = 0;

            Vector3 direction = _boss.playerObject.transform.position - _boss.transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            angle = Mathf.Round(angle * 10f) / 10f;

            if (_boss.playerObject.transform.position.x > _boss.transform.position.x)
            {
                _boss.raserEffect.transform.rotation = Quaternion.Euler(0, 180, -angle); // 오른쪽
                yield return null;
                _boss.raserEffect.transform.DORotate(new Vector3(0, 180, -angle + 80f), 1f);
            }
            else
            {
                _boss.raserEffect.transform.rotation = Quaternion.Euler(0, 0, angle + 180); // 왼쪽
                yield return null;
                _boss.raserEffect.transform.DORotate(new Vector3(0, 0, angle - 80f), 1f);
            }

            _boss.raserEffect.SetActive(true);

            int tickCount = 4;
            float tickInterval = 2f / tickCount;

            for (int tick = 0; tick < tickCount; tick++)
            {
                _boss.DamageCaster2D_Raser.CastDamage((int)(_boss.statSO.damage * _boss.raserDamageValue / tickCount));
                yield return new WaitForSeconds(tickInterval);
            }
            _boss.raserEffect.SetActive(false);

            yield return new WaitForSeconds(0.2f);
        }
        _boss.raserEffect.transform.rotation = Quaternion.identity;
        rb.gravityScale = originalGravity;
        _agentBase.StateMachine.ChangeState(BossStateEnum.Idle);
    }
}
