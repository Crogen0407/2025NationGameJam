using System;
using System.Collections.Generic;
using System.Linq;
using _01.Scripts.SkillSystem;
using UnityEngine;

namespace _01.Scripts.PlayerModeSystem
{
    public class ModeManager : MonoBehaviour
    {
        public static ModeManager Instance;
        public bool _isNewSkill;
        public List<ModeEnum> holdingPlayerModes = new();
        
    
        //모드 에디터
        public PlayerMode[] PlayerModes;
        [HideInInspector] public bool[] toggled;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
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

        public PlayerMode[] GetModes()
        {
            PlayerMode[] playerModes = new PlayerMode[holdingPlayerModes.Count];
            int i = 0;
            foreach (var o in holdingPlayerModes)
            {
                playerModes[i] = PlayerModes[(int)o];
                playerModes[i].modeInfo = PlayerModes[(int)o].modeInfo;
                playerModes[i].Init();
                i++;
            }

            return playerModes;
        }
        
        public void AddMode(ModeEnum mode)
        {
            if(holdingPlayerModes.Any((o)=>o== mode)) return;
            
            _isNewSkill = true;
            holdingPlayerModes.Add(mode); 
        }
    }
}
