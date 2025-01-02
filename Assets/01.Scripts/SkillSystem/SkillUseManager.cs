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
            switch (skill.skillType)
            {
                case SkillEnum.Spray:
                    break;
                case SkillEnum.BombThrow:
                    break;
                case SkillEnum.IceShot:
                    break;
                case SkillEnum.WaterBeam:
                    CurrentSkill = StartCoroutine(WaterBeam(skill));
                    break;
            }
        }

        IEnumerator WaterBeam(Skill skill)
        {
            skill.doGaugeSkillCharge = false;
            while (Input.GetKey(KeyCode.Q) && skill.currentSkillGauge>0)
            {
                //todo: 플레이어 받아오기
                var o = new Player();
                var mobs =Physics2D.CircleCastAll(o.transform.position, 50f, Vector2.down, 3f, LayerMask.GetMask("Monster"));//todo: 몬스터 레이어 받기
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
