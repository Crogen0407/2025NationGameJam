using Crogen.AgentFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : AgentState
{
    public BossIdleState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }
    private readonly int hashIdle = Animator.StringToHash("Idle");

    public override void Enter()
    {
        base.Enter();

        _agentBase.Animator.SetBool(hashIdle, true);
        _agentBase.StartCoroutine(Co_ChageStateToMove());
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
        _agentBase.Animator.SetBool(hashIdle, false);
        base.Exit();
    }

    IEnumerator Co_ChageStateToMove()
    {
        yield return new WaitForSeconds(1f);
        _agentBase.StateMachine.ChangeState(BossStateEnum.Move);
    }
}
