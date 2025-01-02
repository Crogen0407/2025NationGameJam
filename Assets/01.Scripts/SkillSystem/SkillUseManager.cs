using System;
using System.Collections;
using System.Collections.Generic;
using Crogen.CrogenPooling;
using UnityEngine;

namespace _01.Scripts.SkillSystem
{
    public class SkillUseManager : MonoBehaviour
    {
        public static SkillUseManager Instance;
        public Coroutine CurrentSkill;
        [SerializeField] private GameObject smileBombPrefab;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

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
                    StartCoroutine(Spike(skill));
                    break;
                case ModeEnum.Yellow:
                    if (!skill.isUsingSkill)
                    {
                        StartCoroutine(HappyBomb(skill));
                    }
                    break;
                case ModeEnum.Darkblue:
                    break;
                case ModeEnum.Blue:
                    CurrentSkill = StartCoroutine(WaterBeam(skill));
                    break;
                case ModeEnum.Red:
                    StartCoroutine(FireBall(skill));
                    break;
                case ModeEnum.Green:
                    Debug.Log("초록스킬사용~");
                    break;
            }
        }
        
        public void SkillGaugeCharge(Skill skill)
        {
            StartCoroutine(skill.GaugeSkillCharge());
        }

        IEnumerator WaterBeam(Skill skill)
        {
            var o = SkillDebug.Instance.gameObject;
            var dirvec = (o.transform.position - _camera.ScreenToWorldPoint((Vector2)Input.mousePosition));
            float deg = Mathf.Atan2(dirvec.y, dirvec.x) * Mathf.Rad2Deg;
            var ef = gameObject.Pop(EffectPoolType.WaterBeam, o.transform.position, Quaternion.identity);
            ef.gameObject.transform.parent = o.transform;
            var caster = ef.gameObject.GetComponent<DamageCaster2D>();
            while (Input.GetKey(KeyCode.Q) && skill.currentSkillGauge>0)
            {
                ef.gameObject.transform.rotation = Quaternion.Euler(0,0,deg+90);
                skill.doGaugeSkillCharge = false;
                caster.CastDamage(1);
                dirvec = (o.transform.position - _camera.ScreenToWorldPoint((Vector2)Input.mousePosition));
                deg = Mathf.Atan2(dirvec.y, dirvec.x) * Mathf.Rad2Deg;
                yield return null;
                skill.currentSkillGauge-=Time.deltaTime;
            }
            ef.Push();

            skill.doGaugeSkillCharge = true;
            yield break;
        }

        IEnumerator HappyBomb(Skill skill)
        {
            var o = SkillDebug.Instance.gameObject;
            var bomb = Instantiate(smileBombPrefab, o.transform.position + new Vector3(0,1f,0), Quaternion.identity);
            var rb = bomb.gameObject.GetComponent<Rigidbody2D>();
            skill.isUsingSkill = true;
            float elapsedTime = 0;
            rb.Sleep();
            yield return new WaitForSeconds(0.1f);
            while (!Input.GetKeyDown(KeyCode.Q) && !Input.GetMouseButtonDown(0) && elapsedTime < 8)
            {
                bomb.transform.position= o.transform.position + new Vector3(0,1f,0);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            rb.WakeUp();
            skill.isUsingSkill = false;
            rb.AddForce(((Vector2)(_camera.ScreenToWorldPoint((Vector2)Input.mousePosition) - o.transform.position)).normalized * 7.5f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1.5f);
            Destroy(bomb);//폭탄 풀링(해도 되고 안해도 되고-)
            var ef = gameObject.Pop(EffectPoolType.SmileBumbExplosion, bomb.transform.position, Quaternion.identity);
            ef.gameObject.GetComponent<DamageCaster2D>().CastDamage(5);
            yield return new WaitForSeconds(0.3f);
            ef.Push();
        }

        private IEnumerator Spike(Skill skill)
        {
            skill.doGaugeSkillCharge = false;
            var o = SkillDebug.Instance.gameObject;
            var ef = gameObject.Pop(EffectPoolType.SpikeEffect, o.transform.position, Quaternion.identity);
            ef.gameObject.GetComponent<DamageCaster2D>().CastDamage(5);
            while ((Input.GetKey(KeyCode.Q) && skill.currentSkillGauge > 0.2))
            {
                yield return new WaitForSeconds(0.3f);
                ef.Push();
                ef = gameObject.Pop(EffectPoolType.SpikeEffect, o.transform.position, Quaternion.identity);
                ef.gameObject.GetComponent<DamageCaster2D>().CastDamage(5);
                skill.currentSkillGauge -= 0.3f;
            }
            skill.doGaugeSkillCharge = true;
        }

        private IEnumerator FireBall(Skill skill)
        {
            var o = SkillDebug.Instance.gameObject;
            var dirvec = ((Vector2)(_camera.ScreenToWorldPoint((Vector2)Input.mousePosition) - o.transform.position)).normalized;
            Debug.Log(dirvec);
            float deg = Mathf.Atan2(dirvec.y, dirvec.x) * Mathf.Rad2Deg;
            var ef = gameObject.Pop(EffectPoolType.FireBall, o.transform.position, Quaternion.Euler(0,0,deg-90));
            var caster = ef.gameObject.GetComponent<DamageCaster2D>();
            var rb = ef.gameObject.GetComponent<Rigidbody2D>();
            float elapsedTime = 0f;
            rb.AddForce(dirvec * 200f, ForceMode2D.Force);
            while (elapsedTime < 3f)
            {
                caster.CastDamage(4);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            ef.Push();
        }
    }
}
