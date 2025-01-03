using Crogen.AgentFSM;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Crogen.CrogenPooling;
using UnityEngine;

public class EnemyAttackState : AgentState
{
    private readonly Enemy _enemy;
    
    public EnemyAttackState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
        _enemy = agentBase as Enemy;
    }
    private readonly int hashAttack = Animator.StringToHash("Attack");


    public override void Enter()
    {
        // ����
        Debug.Log("���ݰ���");
        base.Enter();
        _agentBase.Movement.StopImmediately();
        //(_agentBase as Enemy).attackEffect.SetActive(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        float dis = (_enemy).ClacPlayerDistance();
        _agentBase.Movement.StopImmediately();

        if (_enemy.playerObject == null || Mathf.Abs(dis) > Mathf.Abs(_enemy.playerAttackDistance))
        {
            _agentBase.StateMachine.ChangeState(EnemyStateEnum.Move);
            _agentBase.Animator.SetBool(hashAttack, false);
        }
        else if (_enemy.currentAttackDelay <= 0)
        {
            
            _enemy.InitAttackDelay();
            Turn(dis);
            //enemy.DamageCaster2D.CastDamage((int)enemy.statSO.damage);
            _agentBase.Animator.SetBool(hashAttack, true);
            //enemy.attackEffect.SetActive(true);
        }
    }

    public override void Exit()
    {
        _enemy.Animator.SetBool(hashAttack, false);
        
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
