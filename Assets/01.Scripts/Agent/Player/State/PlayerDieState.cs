using Crogen.AgentFSM;
using UnityEngine;

public class PlayerDieState : AgentState
{
    private float _currentTime = 0;
    private float _delayTime = 3f;

    private Player _player;
    
    public PlayerDieState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
        _player = agentBase as Player;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _currentTime += Time.unscaledTime;
        if (_currentTime >= _delayTime)
        {
            _currentTime = 0;
            _player.DieScene();
        }
    }
}
