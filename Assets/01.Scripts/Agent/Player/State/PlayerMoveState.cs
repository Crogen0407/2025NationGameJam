using Crogen.AgentFSM;
using Unity.VisualScripting;
using UnityEngine;
using StateMachine = Crogen.AgentFSM.StateMachine;

public class PlayerMoveState : AgentState
{
    private Player _player;
    private PlayerMovement _playerMovement;
    
    public PlayerMoveState(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
        _player = agentBase as Player;
        _playerMovement = agentBase.Movement as PlayerMovement;
    }

    public override void Enter()
    {
        base.Enter();
        _player.InputReader.MoveEvent += OnMove;
    }

    private void OnMove(Vector2 obj)
    {
        
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
