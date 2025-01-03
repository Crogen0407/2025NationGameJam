using System;
using System.Collections.Generic;
using System.Linq;
using _01.Scripts.PlayerModeSystem;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform cameraBounds;
    
    public StageType stageType;
    
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private Transform _platfromParents;
    [SerializeField] private Transform _groundParents;
    [SerializeField] private ModeAdder _modeAdder;
    
    public FollowTarget mirror;
    public FollowTarget mirrorCam;
    
    private List<Enemy> Enemies;
    public int surviveEnemyCount => Enemies.Count;

    private void Start()
    {
        Enemies = GetComponentsInChildren<Enemy>().ToList();
        foreach (Enemy enemy in Enemies)
        {
            DefaultHealthSystem defaultHealthSystem = enemy.healthSystem as DefaultHealthSystem;
            if (defaultHealthSystem != null)
            {
                defaultHealthSystem.dieEvent.AddListener(()=>
                {
                    Enemies.Remove(enemy);
                    StageManager.Instance.StageClearCheck();
                });
            }
        }
    }

    public void Init(StageMaterials materials, StageType type)
    {
        stageType = type;
        
        _background.material = materials.bgMat;
        
        foreach (SpriteRenderer spriteRenderer in _groundParents.GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.material = materials.groundMat;
        
        foreach (SpriteRenderer spriteRenderer in _platfromParents.GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.material = materials.platformMat;
    }

    public void StageClear()
    {
        _modeAdder.Trigger(stageType);
    }
}
