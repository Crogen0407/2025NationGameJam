using System;
using Crogen.CrogenPooling;
using Crogen.PowerfulInput;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _firePointTrm;
    
    [field:SerializeField] public AgentStatSO PlayerStat { get; private set; }
    [field:SerializeField] public InputReader InputReader { get; private set; }

    private float _curDelayTime = 0f;
    
    public event Action<float> OnDelayPercentEvent;
    private bool _canAttack = false;

    private void Awake()
    {
        InputReader.AttackEvent += OnAttack;
    }

    private void Update()
    {
        OnDelayPercentEvent?.Invoke(_curDelayTime/PlayerStat.attackDelay);
        if (_curDelayTime > PlayerStat.attackDelay)
        {
            _canAttack = true;
        }
        else
        {
            _curDelayTime += Time.deltaTime;
        }
    }
    
    public void OnAttack()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(InputReader.MousePosition) - transform.position;
        direction = direction.normalized;        
        PlayerBullet playerBullet = gameObject.Pop(ProjectilePoolType.PlayerBullet, _firePointTrm.position, Quaternion.identity) as PlayerBullet;
        playerBullet.Initialize(direction, 10f, (int)PlayerStat.damage);
    }
}
