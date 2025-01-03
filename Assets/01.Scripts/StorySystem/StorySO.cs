using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StoryData
{
    public Sprite sprite;
    public string description;
}

[CreateAssetMenu(menuName = "SO/StorySO")]
public class StorySO : ScriptableObject
{
    public List<StoryData> storyDataList;
    
    public StoryData this[int index] => storyDataList[index];
}