using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class StageManager : MonoBehaviour
{
    [SerializeField] private List<Stage> _stages;
    private Stage _currentStage;
    
    [SerializeField] private GameEventChannelSO _systemEventChannel;
    [SerializeField] private string _nextSceneName;

    public static StageManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);
    }

    private void Start()
    {
        StageType type = StageSaveData.blockDictionary[StageSaveData.currentKey].stageType;

        List<Stage> typeMatchStage = _stages.Where(x => x.type == type).ToList();
        Stage randomStage = typeMatchStage[Random.Range(0, typeMatchStage.Count)];

        _currentStage = Instantiate(randomStage, transform.position, Quaternion.identity);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
            StageClear();
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
            fadeEvt.isFadeIn = true;

            StageSaveData.isEnd = true;
            _systemEventChannel.AddListener<FadeComplete>(HandleFadeComplete);
            _systemEventChannel.RaiseEvent(fadeEvt);
        }
    }
#endif

    public void StageClearCheck()
    {
        if(_currentStage.surviveEnemyCount > 0)
            return;
        
        StageClear();
    }

    private void StageClear()
    {
        FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
        fadeEvt.isFadeIn = true;

        StageSaveData.blockDictionary[StageSaveData.currentKey].isClear = true;
        _systemEventChannel.AddListener<FadeComplete>(HandleFadeComplete);
        _systemEventChannel.RaiseEvent(fadeEvt);
    }

    private void HandleFadeComplete(FadeComplete evt)
    {
        _systemEventChannel.RemoveListener<FadeComplete>(HandleFadeComplete);
        SceneManager.LoadScene(_nextSceneName);
    }
}
