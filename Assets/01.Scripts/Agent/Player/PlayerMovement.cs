using System;
using Crogen.AgentFSM;
using Crogen.AgentFSM.Movement;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IGroundMovement2D
{
    private Rigidbody2D _rigidbody;
    
    private float Speed { get => _player.AgentStatSO.speed;  set => _player.AgentStatSO.speed = value; }
    
    public Vector3 Velocity { get; set; }
    public Agent AgentBase { get; set; }
    public bool IsGround { get; set; }
    
    private Player _player;
    //Jump
    [SerializeField] private float _jumpPower = 10f;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Vector2 _groundCheckSize;
    [field:SerializeField] public int JumpCount { get; set; } = 1;
    private int _curJumpCount = 0;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); 
    }

    private void Update()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        IsGround = Physics2D.BoxCast(transform.position, _groundCheckSize, 0, Vector2.zero, 0, _whatIsGround);
        if (IsGround && _curJumpCount > 0) _curJumpCount = 0;
    }
    
    public void Initialize(Agent agent)
    {
        AgentBase = agent;
        _player = agent as Player;
    }

    public void SetMovement(Vector3 movement, bool isRotation = true)
    {
        Vector3 finalMovement = movement;
        finalMovement *= Speed;
        
        _rigidbody.velocity = new Vector2(finalMovement.x, _rigidbody.velocity.y);
    }

    public void StopFallingImmediately()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
    }
    
    public void StopImmediately()
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }

    public void OnJump()
    {
        if (_curJumpCount >= JumpCount) return;
        ++_curJumpCount;
        StopFallingImmediately();
        _rigidbody.AddForce(Vector3.up * _jumpPower, ForceMode2D.Impulse);
    }
    
    public void GetKnockback(Vector3 force)
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _groundCheckSize);
        Gizmos.color = Color.white;
    }
}
