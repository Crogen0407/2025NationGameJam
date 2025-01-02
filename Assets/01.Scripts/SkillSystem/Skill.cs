using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;

namespace _01.Scripts.SkillSystem
{
    [Serializable]
    public class Skill
    {
        [FormerlySerializedAs("skillType")] public ModeEnum modeType;
        public string skillName;
        public Sprite skillSprite;
        public float skillTime;
        public float currentSkillTime;
        public bool isUsingSkill;
        
        //게이지 스킬 전용 프로퍼티
        public bool isGaugeSkill;
        public float skillGauge = -1f;
        public float currentSkillGauge;
        public float gaugeSkillChargeSpeed;
        public bool doGaugeSkillCharge = true;
        

        public void Init(Skill skill)
        {
            modeType = skill.modeType;
            skillName = skill.skillName;
            skillSprite = skill.skillSprite;
            skillTime = skill.skillTime;
            isGaugeSkill = skill.isGaugeSkill;
            skillGauge = skill.skillGauge;
            currentSkillGauge = skillGauge;
            gaugeSkillChargeSpeed = skill.gaugeSkillChargeSpeed;
            doGaugeSkillCharge = skill.doGaugeSkillCharge;
            if(isGaugeSkill)
                SkillUseManager.Instance.SkillGaugeCharge(this);
        }

        //스킬 사용용 메서드
        public void Use()
        {
            if (currentSkillTime < skillTime) return;
            SkillUseManager.Instance.Use(this);
        }

        //스킬 UI를 위한 스킬/게이지 스킬 남은 시간/ 잔량 값
        public float GetSkillCoolPercent()
        {
            if (isGaugeSkill)
            {
                return currentSkillGauge / skillGauge;
            }
            else
            {
                return currentSkillTime / skillGauge;
            }
        }
        
        //스킬 쿨다운 코루틴 - currentSkillTime이 skillTime이랑 같아야 스킬 사용 가능(skillTime = 0일시 무시하고 사용)
        public IEnumerator SkillCooldown()
        {
            while (currentSkillTime < skillTime)
            {
                currentSkillTime += Time.deltaTime;
                yield return null;
            }
            currentSkillTime = skillTime;
        }

        //게이지 스킬 전용 게이지 채우는 코루틴
        public IEnumerator GaugeSkillCharge()
        {
            while (true)
            {
                if (doGaugeSkillCharge)
                {
                    if(currentSkillGauge>skillGauge)
                        currentSkillGauge = skillGauge;
                    if (currentSkillGauge < skillGauge)
                    {
                        currentSkillGauge += Time.deltaTime * gaugeSkillChargeSpeed;
                    }
                }
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
