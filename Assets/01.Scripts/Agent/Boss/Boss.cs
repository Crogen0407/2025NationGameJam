using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crogen.AgentFSM;
using Crogen.HealthSystem;
using DG.Tweening;

public class Boss : Agent
{
    public HealthSystem healthSystem { get; private set; }
    [field: SerializeField] public AgentStatSO statSO { get; private set; }

    public GameObject playerObject { get; private set; }
    [field: SerializeField] public float findRadiusValue { get; private set; }
    [field: SerializeField] private LayerMask playerLayer;

    [field: SerializeField] public float playerAttackDistance { get; private set; }
    [field: SerializeField] public Transform attackPos { get; private set; }
    public float currentAttackDelay { get; private set; }

    [field: SerializeField] public DamageCaster2D DamageCaster2D_Raser { get; private set; }
    [field: SerializeField] public GameObject raserEffect { get; set; }
    [field: SerializeField] public float raserDamageValue { get; private set; }
    [field: SerializeField] public List<Transform> raserAttackTransforms { get; private set; } 

    [field: SerializeField] public DamageCaster2D DamageCaster2D_Ground { get; private set; }
    [field: SerializeField] public GameObject groundEffect { get; private set; }

    [field: SerializeField] public DamageCaster2D DamageCoster2D_Front { get; private set; }
    [field: SerializeField] public GameObject frontAttack { get; private set; }

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        Initialize<BossStateEnum>();

        playerObject = null;
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.ChangeState(BossStateEnum.Move);
    }

    protected override void Update()
    {
        base.Update();
        FindPlayer();
    }

    public float ClacPlayerDistance()
    {
        if (playerObject == null) return 0f;
        return transform.position.x - playerObject.transform.position.x;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, findRadiusValue);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerAttackDistance);
    }
}
