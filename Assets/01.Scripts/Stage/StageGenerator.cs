using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SliceType
{
    Row = 0,
    Column = 1,
}

[Serializable]
public struct StageLine
{
    public Vector2Int startPoint;
    public Vector2Int endPoint;
}

public class StageGenerator : MonoBehaviour
{
    [SerializeField] private StageBlock _baseStageBlock;
    
    [SerializeField] private int _sliceCount;
    [SerializeField] private int _stageCount;
    
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    // LeftTop, RightBottom을 Key로 가지는 블록 딕셔너리
    private Dictionary<Tuple<Vector2Int, Vector2Int>, StageBlock> _blockDictionary = 
        new Dictionary<Tuple<Vector2Int, Vector2Int>, StageBlock>();

    private List<StageLine> _rowLineList = new List<StageLine>();
    private List<StageLine> _colLineList = new List<StageLine>();
    private List<StageLine> currentColLines = new List<StageLine>();

    public static StageGenerator Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        Random.InitState(DateTime.Now.Millisecond);
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM("StageSelectScene");
    }

    public void SaveOrLoad()
    {
        if (!StageSaveData.Instance.isSave || StageSaveData.Instance.isReset)
        {
            StageGenerate();
            StageSaveData.Instance.isSave = true;
            StageSaveData.Instance.isReset = false;
            StageSaveData.Instance.blockDictionary = _blockDictionary;
        }
        else
        {
            _blockDictionary = StageSaveData.Instance.blockDictionary;
            LoadStage();
        }
    }

    [ContextMenu("GenerateStage")]
    public void StageGenerate()
    {
        StartCoroutine(GenerateRoutine());
    }

    private IEnumerator GenerateRoutine()
    {
        RowLineInit();
        ColLineInit();
        SliceLine();
        SliceBlock();
        SelectBlock();
        yield return null;
    }

    private void SelectBlock()
    {
        List<StageBlock> blocks = _blockDictionary.Values.ToList();
        int stageIdx = 1;

        if (blocks.Count < _stageCount)
            _stageCount = blocks.Count;
        
        for (int i = 0; i < _stageCount; i++)
        {
            int randIdx = Random.Range(0, blocks.Count - i);
            int lastIdx = blocks.Count - i - 1;

            blocks[randIdx].SetType((StageType)stageIdx);
            
            stageIdx++;
            if (stageIdx >= (int)StageType.Black)
                stageIdx = (int)StageType.None + 1;

            (blocks[randIdx], blocks[lastIdx]) = (blocks[lastIdx], blocks[randIdx]);
        }
    }

    private void SliceLine()
    {
        for (int i = 0; i < _sliceCount; i++)
        {
            SliceType type = (SliceType)Random.Range((int)SliceType.Row, (int)SliceType.Column + 1);
            StageLine line = default;
            
            if (type == SliceType.Row)
            {
                var startColList = _colLineList.Where(x => x.startPoint.x != _width).ToList();
                var startCol = startColList[Random.Range(0, startColList.Count)];
                
                var endColList = _colLineList.Where(x => x.startPoint.y < startCol.endPoint.y 
                                                         && startCol.startPoint.x < x.startPoint.x).ToList();
                var endCol = endColList[Random.Range(0, endColList.Count)];

                int minY = Mathf.Max(startCol.startPoint.y, endCol.startPoint.y);
                int maxY = Mathf.Min(startCol.endPoint.y, endCol.endPoint.y);
                int y = Random.Range(minY, maxY);

                line.startPoint = new Vector2Int(startCol.startPoint.x, y);
                line.endPoint = new Vector2Int(endCol.startPoint.x, y);
                _rowLineList.Add(line);
            }
            else
            {
                var startRowList = _rowLineList.Where(x => x.startPoint.y != _height).ToList();
                var startRow = startRowList[Random.Range(0, startRowList.Count)];
                
                var endRowList = _rowLineList.Where(x => x.startPoint.x < startRow.endPoint.x 
                                                         && startRow.startPoint.y < x.startPoint.y).ToList();
                var endRow = endRowList[Random.Range(0, endRowList.Count)];

                int minX = Mathf.Min(startRow.startPoint.x, endRow.startPoint.x);
                int maxX = Mathf.Max(startRow.endPoint.x, endRow.endPoint.x);
                int x = Random.Range(minX, maxX);

                line.startPoint = new Vector2Int(x, startRow.startPoint.y);
                line.endPoint = new Vector2Int(x,  endRow.startPoint.y);
                _colLineList.Add(line);
            }
        }
    }

    private void SliceBlock()
    {
        foreach (StageLine currentRow in _rowLineList)
        {
            int y = currentRow.startPoint.y;
            List<StageLine> colLine = _colLineList.Where(line => line.startPoint.y <= y 
                                                              && line.endPoint.y > y 
                                                              && line.startPoint.x >= currentRow.startPoint.x
                                                              && currentRow.endPoint.x >= line.startPoint.x).ToList();
            currentColLines = colLine;
            
            foreach (StageLine currentCol in currentColLines)
            {
                var closetColLine = GetMostClosetColLine(currentCol);
                if(closetColLine.startPoint.x == 0)
                    continue;
                var ableRowLines = _rowLineList.Where(line => line.startPoint.x <= currentCol.startPoint.x 
                                                           && closetColLine.startPoint.x <= line.endPoint.x
                                                           && line.startPoint.y > currentRow.startPoint.y).ToList();
                var closetRowLine = GetMostClosetRowLine(ableRowLines);
                
                Tuple<Vector2Int, Vector2Int> LTAndRB =
                    new Tuple<Vector2Int, Vector2Int>(
                        new Vector2Int(currentCol.startPoint.x, currentRow.startPoint.y),
                        new Vector2Int(closetColLine.startPoint.x, closetRowLine.startPoint.y));

                if (!_blockDictionary.ContainsKey(LTAndRB))
                {
                    StageBlock stageBlock = Instantiate(_baseStageBlock, transform);
                    stageBlock.Init(LTAndRB);

                    stageBlock._currentCol = currentCol;
                    stageBlock._currentRow = currentRow;
                    stageBlock._closetCol = closetColLine;
                    stageBlock._closetRow = closetRowLine;
                    
                    StageMoveEffect(stageBlock, new Vector3(LTAndRB.Item1.x + stageBlock.Width * 0.5f, 
                        LTAndRB.Item1.y + stageBlock.Height * 0.5f));
                    _blockDictionary.Add(LTAndRB, stageBlock);
                }
            }
        }
    }

    private void LoadStage()
    {
        foreach (var loadBlock in _blockDictionary)
        {
            StageBlock stageBlock = Instantiate(_baseStageBlock, transform);

            if (loadBlock.Value.isClear)
                stageBlock.isClear = true;
            
            stageBlock.Init(loadBlock.Key);
            StageMoveEffect(stageBlock, new Vector3(loadBlock.Key.Item1.x + stageBlock.Width * 0.5f, 
                loadBlock.Key.Item1.y + stageBlock.Height * 0.5f));
            stageBlock.SetType(loadBlock.Value.stageType);
        }
    }

    private void StageMoveEffect(StageBlock stageBlock, Vector3 targetPos )
    {
        stageBlock.transform.localPosition = Vector3.zero;
        stageBlock.transform.DOLocalMove(targetPos, Random.Range(0.1f, 1f))
            .OnComplete(() => stageBlock.transform.localPosition = targetPos);
    }

    private StageLine GetMostClosetColLine(StageLine currentCol)
    {
        var ableLines = currentColLines.Where(x => x.startPoint.x > currentCol.startPoint.x).ToList();
        if (ableLines.Count == 0)
            return default;
        
        StageLine mostClosetLine = ableLines[0];
        foreach (StageLine stageLine in ableLines)
        {
            if (mostClosetLine.startPoint.x > stageLine.startPoint.x)
                mostClosetLine = stageLine;
        }
        return mostClosetLine;
    }

    private StageLine GetMostClosetRowLine(List<StageLine> lines)
    {
        if (lines.Count == 0)
            return default;
        
        StageLine mostClosetLine = lines[0];
        foreach (StageLine stageLine in lines)
        {
            if (mostClosetLine.startPoint.y > stageLine.startPoint.y)
                mostClosetLine = stageLine;
        }
        return mostClosetLine;
    }

    private void ColLineInit()
    {
        StageLine colLine1 = new StageLine
        {
            startPoint = new Vector2Int(0, 0),
            endPoint = new Vector2Int(0, _height)
        };
        int randX = Random.Range(1, _width);
        StageLine colLine2 = new StageLine
        {
            startPoint = new Vector2Int(randX, 0),
            endPoint = new Vector2Int(randX, _height)
        };
        StageLine colLine3 = new StageLine
        {
            startPoint = new Vector2Int(_width, 0),
            endPoint = new Vector2Int(_width, _height)
        };
        
        _colLineList.Add(colLine1);
        _colLineList.Add(colLine2);
        _colLineList.Add(colLine3);
    }

    private void RowLineInit()
    {
        StageLine rowLine1 = new StageLine
        {
            startPoint = new Vector2Int(0, 0),
            endPoint = new Vector2Int(_width, 0)
        };
        int randY = Random.Range(1, _height);
        StageLine rowLine2 = new StageLine
        {
            startPoint = new Vector2Int(0, randY),
            endPoint = new Vector2Int(_width, randY)
        };
        StageLine rowLine3 = new StageLine
        {
            startPoint = new Vector2Int(0, _height),
            endPoint = new Vector2Int(_width, _height)
        };
        
        _rowLineList.Add(rowLine1);
        _rowLineList.Add(rowLine2);
        _rowLineList.Add(rowLine3);
    }
}
