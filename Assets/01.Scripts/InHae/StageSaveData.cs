using System;
using System.Collections.Generic;
using UnityEngine;

public static class StageSaveData
{
    public static bool isSave;
    public static bool isEnd;
    public static Dictionary<Tuple<Vector2Int, Vector2Int>, Block> blockDictionary = 
        new Dictionary<Tuple<Vector2Int, Vector2Int>, Block>();
    public static Tuple<Vector2Int, Vector2Int> currentKey;
}
