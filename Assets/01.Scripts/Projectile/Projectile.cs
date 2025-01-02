using System;
using Crogen.CrogenPooling;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolingObject
{
    public int Damage { get; set; }
    public Vector2 Direction { get; set; }
    public float Speed { get; set; }
    private DamageCaster2D _damageCaster;
    public string OriginPoolType { get; set; }
    public new GameObject gameObject { get; set; }

    protected virtual void Awake()
    {
        _damageCaster = GetComponent<DamageCaster2D>();
    }

    private void Start()
    {
        _damageCaster.OnCasterSuccessEvent += this.Push;
    }

    public void Initialize(Vector2 direction, float speed, int damage)
    {
        Direction = direction;
        Speed = speed;
        Damage = damage;
    }

    public void OnPop()
    {
    }

    public void OnPush()
    {
    }

    protected virtual void Update()
    {
        Direction = Direction.normalized;
        transform.position += (Vector3)(Direction * Speed) * Time.deltaTime;
    }

    private void LateUpdate()
    {
        _damageCaster.CastDamage(Damage);
    }
}
