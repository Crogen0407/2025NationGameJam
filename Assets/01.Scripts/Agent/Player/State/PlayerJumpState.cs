using Crogen.AgentFSM;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    private readonly Player _player;
    private readonly PlayerMovement _playerMovement;
    private Vector2 _direction;
    public PlayerJumpState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
        _player = agentBase as Player;
        _playerMovement = agentBase.Movement as PlayerMovement;
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
        _direction = Input.GetAxisRaw("Horizontal") * Vector2.right;
        _playerMovement.SetMovement(_direction);
        
        if (_playerMovement.IsGround)
        {
            if(_direction.sqrMagnitude < 0.1f)
                _stateMachine.ChangeState(PlayerStateEnum.Idle);
            else
                _stateMachine.ChangeState(PlayerStateEnum.Move);
        }
        
        if (_direction.sqrMagnitude < 0.1f)
        {
            _playerMovement.StopImmediately();
        }
    }
}
