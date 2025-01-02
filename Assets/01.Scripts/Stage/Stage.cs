using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public StageType type;
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
}
