using Crogen.AgentFSM;
using System;
using System.Collections;
using UnityEngine;

public class EnemyIdleState : AgentState
{
    private Coroutine waitCo;

    public EnemyIdleState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        waitCo = _agentBase.StartCoroutine(Co_WaitChangeStateToMove());
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if ((_agentBase as Enemy).playerObject != null)
        {
            _agentBase.StopCoroutine(waitCo);
            _agentBase.StateMachine.ChangeState(EnemyStateEnum.Move);
        }
    }

    IEnumerator Co_WaitChangeStateToMove()
    {
        float t = UnityEngine.Random.Range(0f, 3f);
        yield return new WaitForSeconds(t);
        _agentBase.StateMachine.ChangeState(EnemyStateEnum.Move);
    }
}
