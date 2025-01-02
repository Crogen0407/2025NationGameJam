using Crogen.AgentFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern3State : AgentState
{
    public BossPattern3State(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }

    private readonly int hashPattern3 = Animator.StringToHash("Pattern3");

    public override void Enter()
    {
        base.Enter();
        _agentBase.StartCoroutine(Co_FrontAttack());
        Debug.Log("3번 패턴 실행");
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
        _agentBase.Animator.SetBool(hashPattern3, false);
        base.Exit();
    }

    IEnumerator Co_FrontAttack()
    {
        Boss boss = _agentBase as Boss;
        _agentBase.Animator.SetBool(hashPattern3, true);
        boss.frontAttack.SetActive(true);
        boss.DamageCoster2D_Front.CastDamage((int)boss.statSO.damage);
        yield return new WaitForSeconds(1);
        boss.frontAttack.SetActive(false);
        _agentBase.Animator.SetBool(hashPattern3, false);
        _agentBase.StateMachine.ChangeState(BossStateEnum.Idle);
    }
}
