using Crogen.AgentFSM;
using UnityEngine;

public class PlayerIdleState : AgentState
{
    private Player _player;
    
    public PlayerIdleState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
        _player = agentBase as Player;
    }

    public override void Enter()
    {
        base.Enter();
        _player.InputReader.MoveEvent += HandleStartMove;
    }

    public override void Exit()
    {
        _player.InputReader.MoveEvent -= HandleStartMove;
        base.Exit();
    }
    
    private void HandleStartMove(Vector2 dir, bool isFirst)
    {
        _stateMachine.ChangeState(PlayerStateEnum.Move);
    }
}
