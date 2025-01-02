using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _01.Scripts.SkillSystem
{
    public class SkillUseManager : MonoBehaviour
    {
        public static SkillUseManager Instance;
        public Coroutine CurrentSkill;

        private void Awake()
        {
            Instance = this;
        }

        public void Use(Skill skill)
        {
            if(skill.skillTime != 0)
                StartCoroutine(skill.SkillCooldown());
            switch (skill.modeType)
            {
                case ModeEnum.Magenta:
                    break;
                case ModeEnum.Yellow:
                    Debug.Log("노랑스킬사용~");
                    break;
                case ModeEnum.Darkblue:
                    break;
                case ModeEnum.Blue:
                    CurrentSkill = StartCoroutine(WaterBeam(skill));
                    break;
                case ModeEnum.Red:
                    Debug.Log("빨강스킬사용~");
                    break;
            }
        }
        
        public void SkillGaugeCharge(Skill skill)
        {
            StartCoroutine(skill.GaugeSkillCharge());
        }

        IEnumerator WaterBeam(Skill skill)
        {
            while (Input.GetKey(KeyCode.Q) && skill.currentSkillGauge>0)
            {
                skill.doGaugeSkillCharge = false;
                //todo: 플레이어 받아오기
                var o = new GameObject();
                var mobs =Physics2D.CircleCastAll(o.transform.position, 3f, o.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition), 3f, LayerMask.GetMask("Monster"));//todo: 몬스터 레이어 받기
                foreach (var m in mobs)
                {
                    var oo = m.transform.gameObject.GetComponent("몬스터~");
                    //todo: 몬스터 피격판정
                    //oo.hit(player.attackPower * 배율) 
                }
                skill.currentSkillGauge-=0.1f;
                yield return new WaitForSeconds(0.1f);//내일의 나: 게이지 스킬 줄어드는 ui는 lerp로 만드셈
            }
            skill.doGaugeSkillCharge = true;
            yield break;
        }
    }
}
