using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public StageType type;
    public List<Enemy> Enemies;

    public int surviveEnemyCount => Enemies.Count(x => !x.isDead);

    private void Awake()
    {
        Enemies = GetComponentsInChildren<Enemy>().ToList();
    }
}
