using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private Image _storyImage;
    [SerializeField] private TextMeshProUGUI _storyText;
    [SerializeField] private StoryListSO _storyListSO;
    private StorySO _currentStory;
    private StoryData _currentStoryData;
    public static int CurrentStoryIndex = 0;
    public int _currentStoryDataIndex = 0;

    private bool _isStoryTelling = false;
    
    public void LoadStory()
    {
        _currentStory = _storyListSO[CurrentStoryIndex];
    }

    public void Next()
    {
        if (_isStoryTelling) return;
        _isStoryTelling = true;
        ++_currentStoryDataIndex;
        _currentStoryData = _storyListSO[_currentStoryDataIndex][_currentStoryDataIndex];
        _storyImage.sprite = _currentStoryData.sprite;
        
        
        
        _storyText.text = _currentStoryData.description;
    }
}
