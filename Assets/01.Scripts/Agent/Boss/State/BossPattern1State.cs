using Crogen.AgentFSM;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BossPattern1State : AgentState
{
    public BossPattern1State(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }

    public readonly int hashJump = Animator.StringToHash("Pattern1");

    public override void Enter()
    {
        base.Enter();
        _agentBase.StartCoroutine(Co_JumpJump());
        Debug.Log("1�� ���� ����");
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
        _agentBase.Animator.SetBool(hashJump, false);
        base.Exit();
    }

    IEnumerator Co_JumpJump() 
    {
        _agentBase.Animator.SetBool(hashJump, true);
        if (Mathf.Approximately(_agentBase.transform.localEulerAngles.y, 0))
        {
            _agentBase.transform.DOJump(new Vector3(_agentBase.transform.position.x - 3, _agentBase.transform.position.y + 2.5f, 0), 2, 2, 0.7f);
        }
        else if (Mathf.Approximately(_agentBase.transform.localEulerAngles.y, 180))
        {
            _agentBase.transform.DOJump(new Vector3(_agentBase.transform.position.x + 3, _agentBase.transform.position.y + 2.5f, 0), 2, 2, 0.7f);
        }

        yield return new WaitForSeconds(0.4f);
        _agentBase.Animator.SetBool(hashJump, false);
        yield return new WaitForSeconds(0.3f);

        _agentBase.transform.DOMoveY(_agentBase.transform.position.y - 3f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        (_agentBase as Boss).groundEffect.SetActive(true);
        (_agentBase as Boss).DamageCaster2D_Ground.CastDamage((int)(_agentBase as Boss).statSO.damage);
        yield return new WaitForSeconds(0.2f);
        (_agentBase as Boss).groundEffect.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        _agentBase.StateMachine.ChangeState(BossStateEnum.Idle);
        yield return null;
    }
}