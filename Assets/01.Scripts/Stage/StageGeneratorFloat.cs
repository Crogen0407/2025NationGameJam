using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public struct StageLineFloat
{
    public Vector2 startPoint;
    public Vector2 endPoint;
}

public class StageGeneratorFloat : MonoBehaviour
{
    [FormerlySerializedAs("stage")] [FormerlySerializedAs("_block")] [SerializeField] private StageBlock stageBlock;
    
    [SerializeField] private int _sliceCount;
    [SerializeField] private int _stageCount;
    
    [SerializeField] private float _width;
    [SerializeField] private float _height;

    // LeftTop, RightBottom을 Key로 가지는 블록 딕셔너리
    private Dictionary<Tuple<Vector2, Vector2>, StageBlock> _blockDictionary =
        new Dictionary<Tuple<Vector2, Vector2>, StageBlock>();

    private List<StageLineFloat> _rowLineList = new List<StageLineFloat>();
    private List<StageLineFloat> _colLineList = new List<StageLineFloat>();
    private List<StageLineFloat> currentColLines = new List<StageLineFloat>();

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

        for (int i = 0; i < _stageCount; i++)
        {
            int randIdx = Random.Range(0, blocks.Count - i);
            int lastIdx = blocks.Count - i - 1;

            blocks[randIdx].SetType((StageType)stageIdx);
            
            stageIdx++;
            if (stageIdx >= (int)StageType.End)
                stageIdx = (int)StageType.None + 1;

            (blocks[randIdx], blocks[lastIdx]) = (blocks[lastIdx], blocks[randIdx]);
        }
    }

    private void SliceLine()
    {
        for (int i = 0; i < _sliceCount; i++)
        {
            SliceType type = (SliceType)Random.Range((int)SliceType.Row, (int)SliceType.Column + 1);
            StageLineFloat line = default;
            
            if (type == SliceType.Row)
            {
                var startColList = _colLineList.Where(x => x.startPoint.x != _width).ToList();
                var startCol = startColList[Random.Range(0, startColList.Count)];
                
                var endColList = _colLineList.Where(x => x.startPoint.y < startCol.endPoint.y 
                                                         && startCol.startPoint.x < x.startPoint.x).ToList();
                var endCol = endColList[Random.Range(0, endColList.Count)];

                float minY = Mathf.Max(startCol.startPoint.y, endCol.startPoint.y);
                float maxY = Mathf.Min(startCol.endPoint.y, endCol.endPoint.y);
                float y = Random.Range(minY, maxY);

                line.startPoint = new Vector2(startCol.startPoint.x, y);
                line.endPoint = new Vector2(endCol.startPoint.x, y);
                _rowLineList.Add(line);
            }
            else
            {
                var startRowList = _rowLineList.Where(x => x.startPoint.y != _height).ToList();
                var startRow = startRowList[Random.Range(0, startRowList.Count)];
                
                var endColList = _rowLineList.Where(x => x.startPoint.x < startRow.endPoint.x 
                                                         && startRow.startPoint.y < x.startPoint.y).ToList();
                var endRow = endColList[Random.Range(0, endColList.Count)];

                float minX = Mathf.Min(startRow.startPoint.x, endRow.startPoint.x);
                float maxX = Mathf.Max(startRow.endPoint.x, endRow.endPoint.x);
                float x = Random.Range(minX, maxX);

                line.startPoint = new Vector2(x, startRow.startPoint.y);
                line.endPoint = new Vector2(x,  endRow.startPoint.y);
                _colLineList.Add(line);
            }
        }
    }

    private void SliceBlock()
    {
        foreach (StageLineFloat currentRow in _rowLineList)
        {
            float y = currentRow.startPoint.y;
            List<StageLineFloat> colLine = _colLineList.Where(line => line.startPoint.y <= y 
                                                                      && line.endPoint.y > y 
                                                                      && line.startPoint.x >= currentRow.startPoint.x
                                                                      && currentRow.endPoint.x >= line.startPoint.x).ToList();
            currentColLines = colLine;
            
            foreach (StageLineFloat currentCol in currentColLines)
            {
                var closetColLine = GetMostClosetColLine(currentCol);
                if(closetColLine.startPoint.x == 0)
                    continue;
                var ableRowLines = _rowLineList.Where(line => line.startPoint.x <= currentCol.startPoint.x 
                                                           && closetColLine.startPoint.x <= line.endPoint.x
                                                           && line.startPoint.y > currentRow.startPoint.y).ToList();
                var closetRowLine = GetMostClosetRowLine(ableRowLines);
                
                Tuple<Vector2, Vector2> LTAndRB =
                    new Tuple<Vector2, Vector2>(
                        new Vector2(currentCol.startPoint.x, currentRow.startPoint.y),
                        new Vector2(closetColLine.startPoint.x, closetRowLine.startPoint.y));

                if (!_blockDictionary.ContainsKey(LTAndRB))
                {
                    StageBlock stageBlock = Instantiate(this.stageBlock, transform);
                    
                    //block.Init(LTAndRB);
                    stageBlock.transform.localPosition = new Vector3(LTAndRB.Item1.x + stageBlock.Width * 0.5f, 
                        LTAndRB.Item1.y + stageBlock.Height * 0.5f);
                    
                    Debug.Log($"{stageBlock.name}, {LTAndRB.Item1}, {LTAndRB.Item2}");
                    _blockDictionary.Add(LTAndRB, stageBlock);
                }
            }
        }
    }

    private StageLineFloat GetMostClosetColLine(StageLineFloat currentCol)
    {
        var ableLines = currentColLines.Where(x => x.startPoint.x > currentCol.startPoint.x).ToList();
        if (ableLines.Count == 0)
            return default;
        
        StageLineFloat mostClosetLine = ableLines[0];
        foreach (StageLineFloat stageLine in ableLines)
        {
            if (mostClosetLine.startPoint.x > stageLine.startPoint.x)
                mostClosetLine = stageLine;
        }
        return mostClosetLine;
    }

    private StageLineFloat GetMostClosetRowLine(List<StageLineFloat> lines)
    {
        if (lines.Count == 0)
            return default;
        
        StageLineFloat mostClosetLine = lines[0];
        foreach (StageLineFloat stageLine in lines)
        {
            if (mostClosetLine.startPoint.y > stageLine.startPoint.y)
                mostClosetLine = stageLine;
        }
        return mostClosetLine;
    }

    private void ColLineInit()
    {
        StageLineFloat colLine1 = new StageLineFloat
        {
            startPoint = new Vector2(0, 0),
            endPoint = new Vector2(0, _height)
        };
        float randX = Random.Range(1, _width);
        StageLineFloat colLine2 = new StageLineFloat
        {
            startPoint = new Vector2(randX, 0),
            endPoint = new Vector2(randX, _height)
        };
        StageLineFloat colLine3 = new StageLineFloat
        {
            startPoint = new Vector2(_width, 0),
            endPoint = new Vector2(_width, _height)
        };
        
        _colLineList.Add(colLine1);
        _colLineList.Add(colLine2);
        _colLineList.Add(colLine3);
    }

    private void RowLineInit()
    {
        StageLineFloat rowLine1 = new StageLineFloat
        {
            startPoint = new Vector2(0, 0),
            endPoint = new Vector2(_width, 0)
        };
        float randY = Random.Range(1, _height);
        StageLineFloat rowLine2 = new StageLineFloat
        {
            startPoint = new Vector2(0, randY),
            endPoint = new Vector2(_width, randY)
        };
        StageLineFloat rowLine3 = new StageLineFloat
        {
            startPoint = new Vector2(0, _height),
            endPoint = new Vector2(_width, _height)
        };
        
        _rowLineList.Add(rowLine1);
        _rowLineList.Add(rowLine2);
        _rowLineList.Add(rowLine3);
    }
}
