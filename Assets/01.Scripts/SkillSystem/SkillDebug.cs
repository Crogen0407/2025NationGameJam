using System;
using System.Collections.Generic;
using _01.Scripts.PlayerModeSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace _01.Scripts.SkillSystem
{
    public class SkillDebug : MonoBehaviour
    {
        private readonly Queue<PlayerMode> Modes = new();
        public PlayerMode currentMode;

        private void Awake()
        {
            Modes.Enqueue(ModeManager.Instance.GetMode(ModeEnum.Blue));
            Modes.Enqueue(ModeManager.Instance.GetMode(ModeEnum.Red));
            Modes.Enqueue(ModeManager.Instance.GetMode(ModeEnum.Yellow));
            currentMode = Modes.Dequeue();
        }

        private void InitSkills()
        {
            foreach (PlayerMode mode in Modes)
            {
                mode.Init();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log($"{currentMode.SkillInstance.skillName} 사용을 시도 ");
                    currentMode.SkillInstance.Use();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (!currentMode.SkillInstance.doGaugeSkillCharge || currentMode.SkillInstance.isUsingSkill) return;
                Modes.Enqueue(currentMode);
                currentMode = Modes.Dequeue();
            }
        }
    }
}
