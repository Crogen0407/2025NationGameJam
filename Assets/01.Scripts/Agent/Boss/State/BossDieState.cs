using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crogen.AgentFSM;

public class BossDieState : AgentState
{
    public BossDieState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        BossMovement bossMovement = _agentBase.GetComponent<BossMovement>();
        bossMovement.enabled = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
