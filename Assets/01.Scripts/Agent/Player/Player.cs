using System;
using Crogen.AgentFSM;
using Crogen.HealthSystem;
using Crogen.PowerfulInput;
using UnityEngine;

public class Player : Agent
{
    [field:SerializeField] public InputReader InputReader { get; private set; }
    [field:SerializeField] public AgentStatSO AgentStatSO { get; private set; }
    private DefaultHealthSystem _healthSystem;
    public Vector2 LookDirection { get; set; }
     
    private void Awake()
    {
        //처음 State는 Idle입니다.
        Initialize<PlayerStateEnum>(); 
        
        //HealthSystem
        _healthSystem = GetComponent<DefaultHealthSystem>();
        _healthSystem.maxHp = AgentStatSO.health;
        _healthSystem.Hp = AgentStatSO.health;
        
        //Movement
        Movement.Initialize(this);
        
        _healthSystem.dieEvent.AddListener(OnDie);
    }

    private void OnDie()
    {
        StateMachine.ChangeState(PlayerStateEnum.Die, true);
    }

    public void HitSound()
    {
        SoundManager.Instance.PlaySFX("PlayerHitSound");
    }
}
