using System;
using Crogen.AgentFSM;
using Crogen.HealthSystem;
using Crogen.PowerfulInput;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Agent
{
    [field:SerializeField] public InputReader InputReader { get; private set; }
    [field:SerializeField] public AgentStatSO AgentStatSO { get; private set; }
    private DefaultHealthSystem _healthSystem;
    public Vector2 LookDirection { get; set; }

    [SerializeField] private GameEventChannelSO _systemChannel;
     
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

    public void DieScene()
    {
        FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
        fadeEvt.isFadeIn = true;

        Debug.Log("ASDSFdasfasd");
        _systemChannel.AddListener<FadeComplete>(HandleFadeComplete);
        _systemChannel.RaiseEvent(fadeEvt);
    }
    
    private void HandleFadeComplete(FadeComplete obj)
    {
        _systemChannel.RemoveListener<FadeComplete>(HandleFadeComplete);
        SceneManager.LoadScene("DeadScene");
    }
}
