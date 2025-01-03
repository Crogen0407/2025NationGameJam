using Crogen.AgentFSM;
using Crogen.CrogenPooling;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BossPattern1State : AgentState
{
    public BossPattern1State(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }

    public readonly int hashJump = Animator.StringToHash("Pattern1");
    private float jumpDamageValue = 2f;

    public override void Enter()
    {
        base.Enter();
        _agentBase.StartCoroutine(Co_JumpJump());
        Debug.Log("1번 패턴 실행");
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
        Boss _boss = _agentBase as Boss;

        _agentBase.Animator.SetBool(hashJump, true);
        if (Mathf.Approximately(_agentBase.transform.localEulerAngles.y, 0))
        {
            _agentBase.transform.DOJump(new Vector3(_agentBase.transform.position.x - 5, _agentBase.transform.position.y + 2.5f, 0), 2, 2, 0.7f);
        }
        else if (Mathf.Approximately(_agentBase.transform.localEulerAngles.y, 180))
        {
            _agentBase.transform.DOJump(new Vector3(_agentBase.transform.position.x + 5, _agentBase.transform.position.y + 2.5f, 0), 2, 2, 0.7f);
        }

        yield return new WaitForSeconds(0.4f);
        _agentBase.Animator.SetBool(hashJump, false);
        yield return new WaitForSeconds(0.3f);

        _agentBase.transform.DOMoveY(_agentBase.transform.position.y - 3f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        (_agentBase as Boss).gameObject.Pop(_boss.groundEffect, _boss.groundPos);

        _boss.DamageCaster2D_Ground.CastDamage((int)(_boss.statSO.damage * jumpDamageValue));
        yield return new WaitForSeconds(0.2f);
        //(_agentBase as Boss).groundEffect.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _agentBase.StateMachine.ChangeState(BossStateEnum.Idle);
        yield return null;
    }
}
