using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _01.Scripts.SkillSystem
{
    [Serializable]
    public class Skill
    {
        public SkillEnum skillType;
        public string skillName;
        public Sprite skillSprite;
        public float skillTime;
        private float _currentSkillTime;

        public void Init(Skill skill)
        {
            skillType = skill.skillType;
            skillName = skill.skillName;
            skillSprite = skill.skillSprite;
            skillTime = skill.skillTime;
            _currentSkillTime = 0f;
        }

        public void Use()
        {
            if (_currentSkillTime < skillTime) return;
            SkillUseManager.Instance.Use(this);
        }
        private IEnumerator SkillCooldown()
        {
            while (_currentSkillTime < skillTime)
            {
                _currentSkillTime += Time.deltaTime;
                yield return null;
            }
            _currentSkillTime = skillTime;
        }
    }
}
