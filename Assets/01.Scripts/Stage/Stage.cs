using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public StageType type;
    public Transform cameraBounds;
    
    [SerializeField] private Material _redMat;
    [SerializeField] private Material _greenMat;
    [SerializeField] private Material _yellowMat;
    [SerializeField] private Material _blueMat;
    [SerializeField] private Transform _groundParents;
    public FollowTarget mirror;
    public FollowTarget mirrorCam;
    
    private List<Enemy> Enemies;
    public int surviveEnemyCount => Enemies.Count(x => !x.isDead);

    private void Awake()
    {
        Enemies = GetComponentsInChildren<Enemy>().ToList();
        
        foreach (Enemy enemy in Enemies)
        {
            DefaultHealthSystem defaultHealthSystem = enemy.healthSystem as DefaultHealthSystem;
            if (defaultHealthSystem != null)
                defaultHealthSystem.dieEvent.AddListener(StageManager.Instance.StageClearCheck);
        }
    }

    private void Start()
    {
        Material material = null;
        
        switch (type)
        {
            case StageType.Red:
                material = _redMat;
                break;
            case StageType.Yellow:
                material = _yellowMat;
                break;
            case StageType.Green:
                material = _greenMat;
                break;
            case StageType.Blue:
                material = _blueMat;
                break;
        }

        foreach (SpriteRenderer spriteRenderer in _groundParents.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.material = material;
        }
    }
}
