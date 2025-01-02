using Crogen.AgentFSM;
using Crogen.AgentFSM.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IGroundMovement2D
{
    private Rigidbody2D rb;

    public bool IsGround { get; set; }
    public Vector3 Velocity { get => rb.velocity; set => rb.velocity = value; }
    public Agent AgentBase { get; set; }

    private void Awake()
    {
        AgentBase = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockback(Vector3 force)
    {
        throw new System.NotImplementedException();
    }

    public void Initialize(Agent agent)
    {
        throw new System.NotImplementedException();
    }

    public void SetMovement(Vector3 movement, bool isRotation = true)
    {
        Velocity = new Vector2(movement.x * (AgentBase as Enemy).statSO.speed, rb.velocity.y);
    }

    public void StopImmediately()
    {
        Velocity = new Vector2(0, rb.velocity.y);
    }
}
