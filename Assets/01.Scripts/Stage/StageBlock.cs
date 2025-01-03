using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public enum StageType
{
    None = 0,
    Red = 1,
    Yellow = 2,
    Green = 3,
    Blue = 4,
    End,
}

public class StageBlock : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameEventChannelSO _cameraEvents;

    [SerializeField] private Material _redMat;
    [SerializeField] private Material _greenMat;
    [SerializeField] private Material _yellowMat;
    [SerializeField] private Material _blueMat;
    
    private SpriteRenderer _spriteRenderer;
    private static bool _isClick;
    
    [HideInInspector] public StageType stageType = StageType.None;
    public Tuple<Vector2Int, Vector2Int> key;
    public bool isClear;
    public float Width { get; private set; }
    public float Height { get; private set; }

    public StageLine _closetCol;
    public StageLine _closetRow;
    public StageLine _currentCol;
    public StageLine _currentRow;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isClick = false;
        
        _spriteRenderer.color = Color.gray;
    }

    public void Init(Tuple<Vector2Int, Vector2Int> LTandRB)
    {
        key = LTandRB;
        Width = LTandRB.Item2.x - LTandRB.Item1.x;
        Height = Mathf.Abs(LTandRB.Item1.y - LTandRB.Item2.y);
        transform.localScale = new Vector3(Width, Height);
    }

    public void SetType(StageType type)
    {
        _spriteRenderer.color = type == StageType.None ? Color.gray : Color.white;
        
        if (isClear)
        {
            _spriteRenderer.DOColor(Color.gray, Random.Range(1f, 2f));
            return;
        }

        if (type != StageType.None)
            _spriteRenderer.sortingOrder = 1;
        
        stageType = type;
        Material material = null;
        
        switch (type)
        {
            case StageType.Red:
                material = _redMat;
                break;
            case StageType.Yellow:
                material = _yellowMat;
                break;
            case StageType.Green:
                material = _greenMat;
                break;
            case StageType.Blue:
                material = _blueMat;
                break;
        }

        if (material != null)
            _spriteRenderer.material = new Material(material);
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

        StageSaveData.Instance.currentKey = key;
        
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
