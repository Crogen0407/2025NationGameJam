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
    private Transform _visualTrm;

    private Player _player;
    
    private void Awake()
    {
        _player = GetComponent<Player>(); 
        _visualTrm = transform.Find("Visual");
        if (_firePointTrm == null) return;
        InputReader.AttackEvent += OnAttack;
    }

    private void OnDestroy()
    {
        if (_firePointTrm == null) return;
        InputReader.AttackEvent -= OnAttack;
    }

    private void Update()
    {
        _player.LookDirection=Camera.main.ScreenToWorldPoint(InputReader.MousePosition) - transform.position;
        _player.LookDirection.Normalize();
        Flip(_player.LookDirection.x < 0);
        if (_firePointTrm == null) return;
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
    
    private void Flip(bool isFlip)
    {
        _visualTrm.rotation = Quaternion.Euler(0, isFlip ? 0 : 180, 0);
    }
    
    public void OnAttack()
    {
        SoundManager.Instance.PlaySFX("ShootSound");
        PlayerBullet playerBullet = gameObject.Pop(ProjectilePoolType.PlayerBullet, _firePointTrm.position, Quaternion.identity) as PlayerBullet;
        playerBullet.Initialize(_player.LookDirection, 10f, (int)PlayerStat.damage);
    }
}
