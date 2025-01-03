using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    
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
        
        StageType type = StageSaveData.Instance.blockDictionary[StageSaveData.Instance.currentKey].stageType;

        List<Stage> typeMatchStage = _stages.Where(x => x.type == type).ToList();
        Stage randomStage = typeMatchStage[Random.Range(0, typeMatchStage.Count)];

        _currentStage = Instantiate(randomStage, transform.position, Quaternion.identity);

        Player player = Instantiate(_player, Vector3.zero, Quaternion.identity);
        
        _virtualCamera.m_Follow = player.transform;
        CinemachineConfiner2D confiner = _virtualCamera.GetComponent<CinemachineConfiner2D>();
        confiner.m_BoundingShape2D = _currentStage.cameraBounds.GetComponent<Collider2D>();
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

            StageSaveData.Instance.isReset = true;
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

        StageSaveData.Instance.blockDictionary[StageSaveData.Instance.currentKey].isClear = true;
        _systemEventChannel.AddListener<FadeComplete>(HandleFadeComplete);
        _systemEventChannel.RaiseEvent(fadeEvt);
    }

    private void HandleFadeComplete(FadeComplete evt)
    {
        _systemEventChannel.RemoveListener<FadeComplete>(HandleFadeComplete);
        SceneManager.LoadScene(_nextSceneName);
    }
}
