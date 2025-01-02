using Crogen.AgentFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern2State : AgentState
{
    public BossPattern2State(Agent agentBase, StateMachine stateMachine, string animBoolName) : base(agentBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _agentBase.StartCoroutine(Co_RaserPattern());
        Debug.Log("2번 패턴 실행");
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
        base.Exit();
    }

    IEnumerator Co_RaserPattern()
    {
        int attackCount = Random.Range(1, 4);
        Boss _boss = _agentBase as Boss;

        for (int i = 0; i < attackCount; i++)
        {
            _boss.raserEffect.SetActive(true);

            int tickCount = 4;
            float tickInterval = 2f / tickCount;

            for (int tick = 0; tick < tickCount; tick++)
            {
                _boss.DamageCaster2D_Raser.CastDamage((int)(_boss.statSO.damage * _boss.raserDamageValue / tickCount));
                yield return new WaitForSeconds(tickInterval);
            }
            _boss.raserEffect.SetActive(false);

            yield return new WaitForSeconds(2f);
            _agentBase.StateMachine.ChangeState(BossStateEnum.Idle);
        }
    }

}
