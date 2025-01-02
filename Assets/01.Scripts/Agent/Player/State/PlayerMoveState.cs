using Crogen.AgentFSM;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    private readonly Player _player;
    private readonly PlayerMovement _playerMovement;
    private Vector2 _direction;
    private bool _isMoving = false;
    
    public PlayerMoveState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
        _player = agentBase as Player;
        _playerMovement = agentBase.Movement as PlayerMovement;
    }

    public override void Enter()
    {
        base.Enter();
        _isMoving = true;
        _player.InputReader.MoveEvent += OnMove;
    }

    private void OnMove(Vector2 dir, bool isMoving)
    {
        _isMoving = isMoving;
    }

    public override void Exit()
    {
        _player.InputReader.MoveEvent -= OnMove;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _direction = Input.GetAxisRaw("Horizontal") * Vector2.right;
        _playerMovement.SetMovement(_direction);
        if(!_isMoving)
        {
            _playerMovement.StopImmediately();
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
