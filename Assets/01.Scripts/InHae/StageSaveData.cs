using System;
using System.Collections.Generic;
using UnityEngine;

public static class StageSaveData
{
    public static bool isSave;
    public static bool isEnd;
    public static Dictionary<Tuple<Vector2Int, Vector2Int>, StageBlock> blockDictionary = 
        new Dictionary<Tuple<Vector2Int, Vector2Int>, StageBlock>();
    public static Tuple<Vector2Int, Vector2Int> currentKey;
}
