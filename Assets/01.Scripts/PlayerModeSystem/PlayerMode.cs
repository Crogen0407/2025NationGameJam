using System;
using _01.Scripts.SkillSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Scripts.PlayerModeSystem
{
    [Serializable]
    public class PlayerMode
    {
        public ModeInfo modeInfo;
        [HideInInspector]public Skill skillInstance;

        public void Init()
        {
            Debug.Log(SkillInfoManager.Instance);
            skillInstance = SkillInfoManager.Instance.GetSkill(modeInfo.mode);
        }
    }
}
