using System;
using System.Collections.Generic;
using System.Linq;
using _01.Scripts.SkillSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _01.Scripts.PlayerModeSystem
{
    public class PlayerModeSystem : MonoBehaviour
    {
        private List<PlayerMode> _modes = new();
        public int ModeLength => _modes.Count;
        public PlayerMode currentMode;
        public PlayerMode nextMode;
        private  int _currentModeIndex = 0;
        public UnityEvent onModeChanged;
        public UnityEvent onModeAdded;
        [SerializeField] private ModeEnum[] startModeForDebug;


        private void Start() 
        {
            _currentModeIndex = 0;
            _modes =ModeManager.Instance.GetModes().ToList();
            currentMode = _modes[0]; 
            nextMode = _modes.Count > 1 ? _modes[1] : null;
            onModeChanged?.Invoke();
        }

        private void InitSkills()
        {
            foreach (PlayerMode mode in _modes)
            {
                mode.Init();
            }
        }

        public void ModeInit(ModeEnum[] modes)
        {
            _currentModeIndex = 0;
            foreach (var o in modes)
            {
                var m = ModeManager.Instance.GetMode(o);
                _modes.Add(m);
            }
            Debug.Log(_modes.Count);
            if(_modes.Count > 1) nextMode = _modes[_currentModeIndex+1]; 
            currentMode = _modes[_currentModeIndex%_modes.Count];
            onModeAdded?.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log($"{_modes[_currentModeIndex%_modes.Count].skillInstance.skillName} 사용을 시도 ");
                _modes[_currentModeIndex%_modes.Count].skillInstance.Use();
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                if (nextMode == null || !_modes[_currentModeIndex%_modes.Count].skillInstance.doGaugeSkillCharge || _modes[_currentModeIndex%_modes.Count].skillInstance.isUsingSkill) return;
                _currentModeIndex++;
                currentMode = _modes[_currentModeIndex%_modes.Count];
                nextMode = _modes[(_currentModeIndex+1)%_modes.Count];
                onModeChanged?.Invoke();
            }
        }
    }
}
