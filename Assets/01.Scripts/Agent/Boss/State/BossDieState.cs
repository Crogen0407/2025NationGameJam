using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crogen.AgentFSM;

public class BossDieState : AgentState
{
    public BossDieState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }
}
