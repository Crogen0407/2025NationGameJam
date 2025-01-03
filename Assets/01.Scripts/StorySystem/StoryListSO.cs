using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StoryList")]
public class StoryListSO : ScriptableObject
{
    public List<StorySO> storySOList;
    
    public StorySO this[int index] => storySOList[index];
}
