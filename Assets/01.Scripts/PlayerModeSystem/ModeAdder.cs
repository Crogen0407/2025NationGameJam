using System;
using _01.Scripts.SkillSystem;
using UnityEngine;

namespace _01.Scripts.PlayerModeSystem
{
    public class ModeAdder : MonoBehaviour
    {
        public bool isUsed = false;
        public void Trigger(StageType mode)
        {
            ModeEnum modeEnum = (ModeEnum)mode;
            switch (mode)
            {
                case StageType.Red:
                    modeEnum = ModeEnum.Red;
                    break;
                case StageType.Yellow:
                    modeEnum = ModeEnum.Yellow;
                    break;
                case StageType.Green:
                    modeEnum = ModeEnum.Green;
                    break;
                case StageType.Blue:
                    modeEnum = ModeEnum.Blue;
                    break;
            }
            ModeManager.Instance.AddMode(modeEnum);
            isUsed = true;
        }
    }
}
