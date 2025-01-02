using System;
using UnityEngine;

namespace _01.Scripts.SkillSystem
{
    public class SkillUseManager : MonoBehaviour
    {
        public static SkillUseManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Use(Skill skill)
        {
            switch (skill.skillType)
            {
                case SkillEnum.Spray:
                    break;
                case SkillEnum.BombThrow:
                    break;
                case SkillEnum.IceShot:
                    break;
                case SkillEnum.WaterBeam:
                    break;
            }
        }
    }
}
