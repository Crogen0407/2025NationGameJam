using System;
using _01.Scripts.SkillSystem;
using UnityEngine;

namespace _01.Scripts.PlayerModeSystem
{
    [Serializable]
    public class PlayerMode
    {
        public ModeInfo ModeInfo;
        public Skill SkillInstance;

        public void Init()
        {
            SkillInstance = SkillInfoManager.Instance.GetSkill(ModeInfo.mode);
        }
    }
}
