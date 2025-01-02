using Crogen.AgentFSM;
using Crogen.HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    public HealthSystem healthSystem { get; private set; }
    [field: SerializeField] public AgentStatSO statSO { get; private set; }

    public GameObject playerObject { get; private set; }
    [field: SerializeField] public float radiusValue { get; private set; }
    [field: SerializeField] private LayerMask playerLayer;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        Initialize<EnemyStateEnum>();

        playerObject = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusValue);
    }

    public void FindPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radiusValue, playerLayer);
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
}
