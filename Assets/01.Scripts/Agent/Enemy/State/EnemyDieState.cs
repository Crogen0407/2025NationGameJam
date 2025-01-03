using Crogen.AgentFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : AgentState
{
    public EnemyDieState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }
    public override void Enter()    
    {
        base.Enter();
        EnemyMovement enemyMovement = _agentBase.GetComponent<EnemyMovement>();
        enemyMovement.enabled = false;
        SoundManager.Instance.PlaySFX("EnemyDieSound");
        Debug.Log("dfsf");
        (_agentBase as Enemy).isDead = true;
        (_agentBase as Enemy).
            DestroySelf();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
