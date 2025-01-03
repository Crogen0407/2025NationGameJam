using System.Collections.Generic;
using _01.Scripts.PlayerModeSystem;
using DG.Tweening;
using UnityEngine;

namespace _01.Scripts.SkillSystem
{
    public class SkillDebug : MonoBehaviour
    {
        private readonly Queue<PlayerMode> Modes = new();
        public PlayerMode currentMode;
        public static SkillDebug Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Modes.Enqueue(ModeManager.Instance.GetMode(ModeEnum.Blue));
            Modes.Enqueue(ModeManager.Instance.GetMode(ModeEnum.Red));
            Modes.Enqueue(ModeManager.Instance.GetMode(ModeEnum.Yellow));
            Modes.Enqueue(ModeManager.Instance.GetMode(ModeEnum.Magenta));
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
                Debug.Log($"{currentMode.skillInstance.skillName} 사용을 시도 ");
                    currentMode.skillInstance.Use();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (!currentMode.skillInstance.doGaugeSkillCharge || currentMode.skillInstance.isUsingSkill) return;
                Modes.Enqueue(currentMode);
                currentMode = Modes.Dequeue();
                Debug.Log(currentMode.modeInfo.mode.ToString());
            }

            if (Input.GetKey(KeyCode.A))
            {
                gameObject.transform.DOMoveX(0, 0, true);
            }
        }
    }
}
