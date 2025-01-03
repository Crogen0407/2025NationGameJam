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
    private readonly int hashAttack = Animator.StringToHash("Attack");


    public override void Enter()
    {
        // 공격
        Debug.Log("공격가능");
        base.Enter();
        _agentBase.Movement.StopImmediately();
        //(_agentBase as Enemy).attackEffect.SetActive(true);
    }

    public override void UpdateState()
    {
        Enemy enemy = _agentBase as Enemy;

        base.UpdateState();
        float dis = (enemy).ClacPlayerDistance();
        _agentBase.Movement.StopImmediately();

        if (enemy.playerObject == null || Mathf.Abs(dis) > Mathf.Abs(enemy.playerAttackDistance))
        {
            _agentBase.StateMachine.ChangeState(EnemyStateEnum.Move);
            _agentBase.Animator.SetBool(hashAttack, false);
        }
        else if (enemy.currentAttackDelay <= 0)
        {
            
            enemy.InitAttackDelay();
            Turn(dis);
            //enemy.DamageCaster2D.CastDamage((int)enemy.statSO.damage);
            _agentBase.Animator.SetBool(hashAttack, true);
            //enemy.attackEffect.SetActive(true);
        }
    }

    public override void Exit()
    {
        _agentBase.Animator.SetBool(hashAttack, false);
        (_agentBase as Enemy).attackEffect.SetActive(false);

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
