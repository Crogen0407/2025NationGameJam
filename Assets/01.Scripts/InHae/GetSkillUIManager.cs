using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.PlayerModeSystem;
using _01.Scripts.SkillSystem;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class GetSkillUIManager : MonoBehaviour
{
    [SerializeField] private Transform _green;
    [SerializeField] private Transform _blue;
    [SerializeField] private Transform _red;
    [SerializeField] private Transform _yellow;
    
    public static GetSkillUIManager Instance;

    private CanvasGroup _group;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _group = GetComponent<CanvasGroup>();
    }

    public void Show(StageType type)
    {
        switch (type)
        {
            case StageType.Red:
                _red.gameObject.SetActive(true);
                break;
            case StageType.Yellow:
                _yellow.gameObject.SetActive(true);
                break;
            case StageType.Green:
                _green.gameObject.SetActive(true);
                break;
            case StageType.Blue:
                _blue.gameObject.SetActive(true);
                break;
        }

        ModeManager.Instance._isNewSkill = false;
        _group.DOFade(1f, 0.3f);
        DOVirtual.DelayedCall(2f, () => _group.DOFade(0f, 0.3f));
    }
}
