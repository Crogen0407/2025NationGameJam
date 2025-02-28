using Crogen.AgentFSM;
using Crogen.CrogenPooling;
using Crogen.HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    public HealthSystem healthSystem { get; private set; }
    [field: SerializeField] public AgentStatSO statSO { get; private set; }
    [field: SerializeField] public DamageCaster2D DamageCaster2D { get; private set; }

    public GameObject playerObject { get; private set; }
    [field: SerializeField] public float findRadiusValue { get; private set; }
    [field: SerializeField] private LayerMask playerLayer;

    [field: SerializeField] public float playerAttackDistance { get; private set; }
    public float currentAttackDelay { get; private set; }
    [field: SerializeField] public EffectPoolType AttackEffectPoolType;

    private void Awake()
    {
        Initialize<EnemyStateEnum>();
        healthSystem = GetComponent<DefaultHealthSystem>();

        playerObject = null;
        (healthSystem as DefaultHealthSystem).dieEvent.AddListener(OnDie);
    }
    
    private void OnDie()
    {
        Debug.Log("ddf");
        StateMachine.ChangeState(EnemyStateEnum.Die, true);
    }
    protected override void Start()
    {
        base.Start();
        currentAttackDelay = 0;
    }

    protected override void Update()
    {
        base.Update();
        FindPlayer();
        if (currentAttackDelay > 0)
        {
            currentAttackDelay -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, findRadiusValue);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerAttackDistance);
    }

    public void FindPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, findRadiusValue, playerLayer);
        if (playerCollider != null)
        {
            playerObject = playerCollider.gameObject;
        }
        else
        {
            playerObject = null;
        }
    }

    public float ClacPlayerDistance()
    {
        if (playerObject == null) return 0f;
        return transform.position.x - playerObject.transform.position.x;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void InitAttackDelay()
    {
        currentAttackDelay = statSO.attackDelay;
    }

    public void HitSound()
    {
        SoundManager.Instance.PlaySFX("PlayerHitSound");
    }
}
