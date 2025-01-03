using Crogen.AgentFSM;
using UnityEngine;

public class PlayerDieState : AgentState
{
    private float _currentTime = 0;
    private float _delayTime = 3f;
    
    public PlayerDieState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _currentTime += Time.deltaTime;
        if (_currentTime >= _delayTime)
        {
            _currentTime = 0;
            
        }
    }
}
