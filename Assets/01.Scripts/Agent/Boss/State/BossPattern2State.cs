using Crogen.AgentFSM;
using Crogen.AgentFSM.Boss;
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

        for (int i = 0; i < _boss.raserAttackTransforms.Count; i++)
        {
            Raser raser = _boss.SpawnSkillObject(_boss.raserEffect, _boss.raserAttackTransforms[i], Quaternion.identity).GetComponent<Raser>();
            raser.damage = _boss.statSO.damage;
        }

        yield return null;
        _agentBase.StateMachine.ChangeState(BossStateEnum.Idle);
    }
}
