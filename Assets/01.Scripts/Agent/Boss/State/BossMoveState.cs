using Crogen.AgentFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveState : AgentState
{
    public BossMoveState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }
    private readonly int hashMove = Animator.StringToHash("Move");
    private Vector3 dir;
    private float patternCool;

    public override void Enter()
    {
        base.Enter();
        patternCool = Random.Range(0.5f, 3f);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _agentBase.Animator.SetBool(hashMove, true);
        if ((_agentBase as Boss).playerObject != null)
        {
            float dis = (_agentBase as Boss).ClacPlayerDistance();

            if (Mathf.Abs(dis) < Mathf.Abs((_agentBase as Boss).playerAttackDistance))
            {
                _agentBase.Movement.StopImmediately();
            }
            else
            {
                Turn(dis);
            }
        }

        _agentBase.Movement.SetMovement(dir, true);

        UsePattern();
    }

    public override void Exit()
    {
        _agentBase.Animator.SetBool(hashMove, false);
    }

    private void Turn(float _dir)
    {
        if (_dir < 0)
        {
            dir = Vector3.right;
            _agentBase.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (_dir > 0)
        {
            dir = Vector3.left;
            _agentBase.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_dir == 0) return;
    }

    private void UsePattern()
    {
        if (patternCool <= 0)
        {
            if ((_agentBase as Boss).playerObject == null) return;
            Debug.Log("패턴 실행");
            float dis = Vector3.Distance(_agentBase.transform.position, (_agentBase as Boss).playerObject.transform.position);
            if (dis > 6)
            {
                if (!RadomEventPattern(2)) _agentBase.StateMachine.ChangeState(BossStateEnum.Pattern1);
                else _agentBase.StateMachine.ChangeState(BossStateEnum.Pattern4);
            }
            else
            {
                if (!RadomEventPattern(10)) _agentBase.StateMachine.ChangeState(BossStateEnum.Pattern3);
                else if (RadomEventPattern(10))_agentBase.StateMachine.ChangeState(BossStateEnum.Pattern2);
                else if (RadomEventPattern(10))_agentBase.StateMachine.ChangeState(BossStateEnum.Pattern3);
            }
        }
        else if (patternCool > 0)
        {
            Debug.Log("패턴 대기 중");
            patternCool -= Time.deltaTime;
        }
    }

    private bool RadomEventPattern(int percent)
    {
        int index = Random.Range(0, percent);

        if(index == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
