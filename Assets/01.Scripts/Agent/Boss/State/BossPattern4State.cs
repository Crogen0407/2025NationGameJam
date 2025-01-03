using Crogen.AgentFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern4State : AgentState
{
    public BossPattern4State(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }
    private Rigidbody2D rb;

    public override void Enter()
    {
        base.Enter();
        rb = _agentBase.GetComponent<Rigidbody2D>();
        Debug.Log("4번 패턴 실행");
        _agentBase.StartCoroutine(Dash());
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    IEnumerator Dash()
    {
        float originalVelocity = rb.velocity.x;
        rb.velocity = new Vector2(rb.velocity.x * 4f, rb.velocity.y);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;
        _agentBase.StateMachine.ChangeState(BossStateEnum.Idle);
    }
}
