using Crogen.CrogenPooling;
using UnityEngine;

public class PlayerBullet : Projectile
{
    private readonly int _colorID = Shader.PropertyToID("_Color");
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _lifeTime = 10f;
    private float _curTime = 0;
    
    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponentInChildren(typeof(SpriteRenderer)) as SpriteRenderer;
    }


    public override void OnPush()
    {
        gameObject.Pop(EffectPoolType.PlayerBulletExplosionEffect, transform.position, Quaternion.identity); 
    }

    protected override void Update()
    {
        base.Update();
        _curTime += Time.deltaTime;
        if (_curTime >= _lifeTime)
        {
            _curTime = 0;
            this.Push();
        }
    }
    
    public void SetColor(Color color)
    {
        _spriteRenderer.material.SetColor(_colorID, color);
    }
}
