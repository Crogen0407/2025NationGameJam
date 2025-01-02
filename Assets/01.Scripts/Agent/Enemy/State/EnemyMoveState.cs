using Crogen.AgentFSM;
using Crogen.AgentFSM.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : AgentState
{
    private float maxTurnTime = 2f;
    private float turnTime = 0f;
    private Vector3 dir;

    public EnemyMoveState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        turnTime = maxTurnTime;
        Turn();
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
            float dis = (_agentBase as Enemy).ClacPlayerDistance();
            
            if (Mathf.Abs(dis) < Mathf.Abs((_agentBase as Enemy).playerAttackDistance)) 
            { 
                _agentBase.Movement.StopImmediately();
                _agentBase.StateMachine.ChangeState(EnemyStateEnum.Attack);
            }
            else
            {
                Turn(dis);
            }
        }
        else if ((_agentBase as Enemy).playerObject == null)
        {
            if (turnTime >= 0)
            {
                turnTime -= Time.deltaTime;
            }
            else
            {
                Turn();
                turnTime = maxTurnTime;
            }
        }
        _agentBase.Movement.SetMovement(dir, true);
    }

    private void Turn()
    {
        int _dir = Random.Range(0, 2);

        if (_dir == 0)
        {
            dir = Vector3.right;
            _agentBase.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (_dir == 1)
        {
            dir = Vector3.left;
            _agentBase.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
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
}
