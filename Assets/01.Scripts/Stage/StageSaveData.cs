using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class StageSaveData : MonoBehaviour
{
    public static StageSaveData Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    public bool isSave;
    public bool isReset;
    public Dictionary<Tuple<Vector2Int, Vector2Int>, StageBlock> blockDictionary = new ();
    public Tuple<Vector2Int, Vector2Int> currentKey;
    public StageBlock currentStage
    {
        get
        {
            if (currentKey == null)
                return null;
            
            return blockDictionary[currentKey];
        }
    }
}
