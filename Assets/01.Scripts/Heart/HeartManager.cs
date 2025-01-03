using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _01.Scripts.PlayerModeSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[System.Serializable]
public class HeartList
{
    public GameObject heartPiece;
    public Color pieceColor;
}

public class HeartManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemEventChannel;
    [SerializeField] private string _nextSceneName;
    
    [SerializeField] private Transform _heart;
    [field: SerializeField] private Camera mainCamera;
    [field: SerializeField] List<HeartList> heartPiece = new List<HeartList>();

    private bool _isPainting;
    private bool _disabling;
    private bool _isInit;
    private Coroutine _coroutine;

    private HeartPart _currentHeart;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; 
        }
    }

    private void Start()
    {
        StartBlackHeart();
        // if(HeartDataManager.instance != null)
        // {
        //     for(int i = 0; i < heartPiece.Count; i++)
        //     {
        //         heartPiece[i].pieceColor = HeartDataManager.instance.heartColorList[i];
        //         if (i == 0)
        //             PaintColor(heartPiece[i].heartPiece, HeartDataManager.instance.heartColorList[i], () => _isInit = true);
        //         else
        //             PaintColor(heartPiece[i].heartPiece, HeartDataManager.instance.heartColorList[i]);
        //     }
        // }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SelectHeartPiece();
        }
    }

    private void SelectHeartPiece()
    {
        if(_disabling || !_isInit || _isPainting)
            return;
        
        if (!StageSaveData.Instance.isSave)
        {
            StartDisable();
        }
        
        if (StageSaveData.Instance.currentKey == null)
            return;
        
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        
        if (hit.collider != null)
        {
            Color color = GetStageColor();

            if (hit.collider.TryGetComponent(out HeartPart heartPart))
            {
                for (int i = 0; i < heartPiece.Count; i++)
                {
                    var heart = heartPiece[i];
                     
                    if (heart.heartPiece == hit.collider.gameObject)
                    {
                        if (heart.pieceColor != Color.white)
                            return;
                        SoundManager.Instance.PlaySFX("ButtonClick");
                        _currentHeart = heartPart;
                        HeartPart.isClick = true;
                        _currentHeart.SmallSize();
                        
                        HeartDataManager.instance.SaveColor(color, i);
                        int currentClearCnt = HeartDataManager.instance.heartColorList.Count(x => x != Color.white);
                        // 하트 다 채웠을 때
                        if (currentClearCnt == heartPiece.Count)
                        {
                            PaintColor(hit.collider.gameObject, color, StartBlackHeart);
                        }
                        else
                        {
                            PaintColor(hit.collider.gameObject, color, () =>
                            {
                                if (ModeManager.Instance._isNewSkill)
                                {
                                    DOVirtual.DelayedCall(0.3f,
                                        () => GetSkillUIManager.Instance.Show(StageSaveData.Instance.currentStage
                                            .stageType));
                                }

                                StartDisable();
                            });
                        }
                    }
                }
            }
        }
    }

    private Color GetStageColor()
    {
        Color color = Color.red;
        switch (StageSaveData.Instance.currentStage.stageType)
        {
            case StageType.Red:
                color  = Color.red;
                break;
            case StageType.Yellow:
                color  = Color.yellow;
                break;
            case StageType.Green:
                color  = Color.green;
                break;
            case StageType.Blue:
                color  = Color.blue;
                break;
        }

        return color;
    }

    private void PaintColor(GameObject piece, Color color, Action action = null)
    {
        _isPainting = true;
        
        piece.GetComponent<SpriteRenderer>().DOColor(color, 0.8f).OnComplete(()=>
        {
            action?.Invoke();
            _isPainting = false;
        });
    }

    private void StartDisable()
    {
        _disabling = true;
        if (_coroutine != null)
            return;
        _coroutine = StartCoroutine(HeartDisable());
    }
    
    private void StartBlackHeart()
    {
        _disabling = true;
        if (_coroutine != null)
            return;
        _coroutine = StartCoroutine(BlackHeart());
    }
    
    private IEnumerator HeartDisable()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < heartPiece.Count; i++)
        {
            var heart = heartPiece[i];
            if (i == heartPiece.Count - 1)
            {
                heart.heartPiece.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).OnComplete(() 
                    => StageGenerator.Instance.SaveOrLoad());
                _coroutine = null;
            }
            else
            {
                heart.heartPiece.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f);
            }
        }
    }
    
    private IEnumerator BlackHeart()
    {
        yield return new WaitForSeconds(0.8f);
        
        for (int i = 0; i < heartPiece.Count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            SoundManager.Instance.PlaySFX("BlackHeart");
            var heart = heartPiece[i];
            
            if (i == heartPiece.Count - 1)
            {
                heart.heartPiece.GetComponent<SpriteRenderer>().DOColor(Color.black, 0.5f)
                    .OnComplete(()=> DOVirtual.DelayedCall(1f, HeartBrokenAction));
                _coroutine = null;
            }
            else
            {
                heart.heartPiece.GetComponent<SpriteRenderer>().DOColor(Color.black, 0.5f);
            }

            Vector3 defaultPos = heart.heartPiece.transform.position;
            heart.heartPiece.transform.DOShakePosition(0.8f, 0.1f).OnComplete(() => transform.position = defaultPos);
        }
    }

    private void HeartBrokenAction()
    {
        _heart.DOShakePosition(3f, 0.5f).OnComplete(() =>
        {
            SoundManager.Instance.PlaySFX("BrokenHeart");
            for (int i = 0; i < heartPiece.Count(); i++)
            {
                Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                Rigidbody2D rigidbody2D = heartPiece[i].heartPiece.GetComponent<Rigidbody2D>();
                rigidbody2D.AddForce(dir * 5f, ForceMode2D.Impulse);
                rigidbody2D.gravityScale = 1.5f;
            }

            DOVirtual.DelayedCall(2f, () =>
            {
                FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
                fadeEvt.isFadeIn = true;
                _systemEventChannel.AddListener<FadeComplete>(HandleFadeComplete);
                _systemEventChannel.RaiseEvent(fadeEvt);
            });
        });
    }
    
    
    private void HandleFadeComplete(FadeComplete obj)
    {
        _systemEventChannel.RemoveListener<FadeComplete>(HandleFadeComplete);
        SceneManager.LoadScene(_nextSceneName);
    }
}
