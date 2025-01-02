using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Block : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameEventChannelSO _cameraEvents;
    
    private SpriteRenderer _spriteRenderer;
    private static bool _isClick;
    
    //Debug
    public Tuple<Vector2Int, Vector2Int> key;
    public StageType stageType = StageType.None;
    public bool isClear;
    public float Width { get; private set; }
    public float Height { get; private set; }
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isClick = false;
    }

    public void Init(Tuple<Vector2Int, Vector2Int> key)
    {
        this.key = key;
        Width = key.Item2.x - key.Item1.x;
        Height = Mathf.Abs(key.Item1.y - key.Item2.y);
        transform.localScale = new Vector3(Width, Height);
    }

    public void SetType(StageType type)
    {
        if (isClear)
        {
            _spriteRenderer.color = Color.gray;
            return;
        }
        
        stageType = type;
        _spriteRenderer.sortingOrder = 1;
        switch (type)
        {
            case StageType.Red:
                _spriteRenderer.color = new Color(200,0,0);
                break;
            case StageType.Yellow:
                _spriteRenderer.color = Color.yellow;
                break;
            case StageType.Green:
                _spriteRenderer.color = Color.green;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(stageType == StageType.None || _isClick)
            return;

        _isClick = true;
        
        var evt = CameraEvents.CameraZoomInEvent;
        Vector3 targetPos = transform.position;
        targetPos.z = -10;
        evt.targetPos = targetPos;
        
        evt.moveTime = 1f;
        evt.zoomInTime = 1f;
        float lensSize = 0.01f;
        evt.lensSize = lensSize;

        StageSaveData.currentKey = key;
        Debug.Log(StageSaveData.currentKey);
        
        _cameraEvents.RaiseEvent(evt);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(stageType == StageType.None || _isClick)
            return;

        _spriteRenderer.sortingOrder = 2;
        Vector3 targetScale = new Vector3(Width + 0.5f, Height + 0.5f);
        transform.DOScale(targetScale, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(stageType == StageType.None || _isClick)
            return;
        _spriteRenderer.sortingOrder = 1;
        Vector3 targetScale = new Vector3(Width, Height);
        transform.DOScale(targetScale, 0.2f);
    }
}
