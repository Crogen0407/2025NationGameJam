using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.PlayerModeSystem;
using _01.Scripts.SkillSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class ModeManager : MonoBehaviour
{
    public static ModeManager Instance;
    
    //모드 에디터
    public PlayerMode[] PlayerModes;
    [HideInInspector] public bool[] toggled;

    private void Awake()
    {
        Instance = this;
    }

    public PlayerMode GetMode(ModeEnum modeEnum)
    {
        var mode = new PlayerMode
        {
            modeInfo = PlayerModes[(int)modeEnum].modeInfo
        };
        mode.Init();
        return mode;
    }
}
