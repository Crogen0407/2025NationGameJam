using System;
using Crogen.AgentFSM;
using Crogen.HealthSystem;
using Crogen.PowerfulInput;
using UnityEngine;

public class Player : Agent
{
    [field:SerializeField] public InputReader InputReader { get; private set; }
    [field:SerializeField] public AgentStatSO AgentStatSO { get; private set; }
    private HealthSystem _healthSystem;
    public Vector2 LookDirection { get; set; }
     
    private void Awake()
    {
        //처음 State는 Idle입니다.
        Initialize<PlayerStateEnum>(); 
        
        //HealthSystem
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.maxHp = AgentStatSO.health;
        _healthSystem.Hp = AgentStatSO.health;
        
        //Movement
        Movement.Initialize(this);
    }

    public void HitSound()
    {
        SoundManager.Instance.PlaySFX("PlayerHitSound");
    }
}
