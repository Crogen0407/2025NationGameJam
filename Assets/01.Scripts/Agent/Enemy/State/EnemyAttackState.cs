using Crogen.AgentFSM;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyAttackState : AgentState
{
    public EnemyAttackState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        // 공격
        Debug.Log("공격가능");
        base.Enter();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        float dis = (_agentBase as Enemy).ClacPlayerDistance();

        if ((_agentBase as Enemy).playerObject == null || Mathf.Abs(dis) > Mathf.Abs((_agentBase as Enemy).playerAttackDistance))
        {
            _agentBase.StateMachine.ChangeState(EnemyStateEnum.Move);
        }
        else if ((_agentBase as Enemy).currentAttackDelay <= 0)
        {
            _agentBase.Movement.StopImmediately();
            (_agentBase as Enemy).InitAttackDelay();
            Turn(dis);
            Debug.Log("공격하기");
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    IEnumerator ReturnIdle()
    {
        yield return new WaitForSeconds(1f);
        _agentBase.StateMachine.ChangeState(EnemyStateEnum.Idle);
    }

    private void Turn(float _dir)
    {
        if (_dir < 0)
        {
            _agentBase.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (_dir > 0)
        {
            _agentBase.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_dir == 0) return;
    }
}
