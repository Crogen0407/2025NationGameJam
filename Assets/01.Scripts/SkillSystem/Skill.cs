using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
        
        //게이지 스킬 전용 프로퍼티
        public bool isGaugeSkill;
        public float skillGauge = -1f;
        public float currentSkillGauge;
        public float gaugeSkillChargeSpeed;
        public bool doGaugeSkillCharge = true;
        

        public void Init(Skill skill)
        {
            skillType = skill.skillType;
            skillName = skill.skillName;
            skillSprite = skill.skillSprite;
            skillTime = skill.skillTime;
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

        private IEnumerator GaugeSkillCharge()
        {
            while (true)
            {
                if (currentSkillGauge < skillGauge && doGaugeSkillCharge)
                {
                    currentSkillGauge += Time.deltaTime * gaugeSkillChargeSpeed;
                }
                else if(currentSkillGauge>skillGauge)
                    currentSkillGauge = skillGauge;
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
