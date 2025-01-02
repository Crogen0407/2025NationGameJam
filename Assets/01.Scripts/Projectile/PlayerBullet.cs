using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Projectile
{
    private readonly int _colorID = Shader.PropertyToID("_Color");
    private SpriteRenderer _spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponentInChildren(typeof(SpriteRenderer)) as SpriteRenderer;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.material.SetColor(_colorID, color);
    }
}
