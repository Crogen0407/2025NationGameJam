using System.Collections;
using System.Collections.Generic;
using Crogen.AgentFSM;
using UnityEngine;

public class PlayerGroundState : AgentState
{
    private readonly Player _player;
    private readonly PlayerMovement _playerMovement;
    
    public PlayerGroundState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
        _player = agentBase as Player;
        _playerMovement = agentBase.Movement as PlayerMovement;
    }

    public override void Enter()
    {
        base.Enter();
        _player.InputReader.JumpEvent += OnJump;
    }

    private void OnJump()
    {
        _playerMovement.OnJump();
        _stateMachine.ChangeState(PlayerStateEnum.Jump);
    }

    public override void Exit()
    {
        _player.InputReader.JumpEvent -= OnJump;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
