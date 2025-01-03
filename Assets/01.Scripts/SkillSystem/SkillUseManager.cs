using System.Collections;
using Crogen.CrogenPooling;
using DG.Tweening;
using UnityEngine;

namespace _01.Scripts.SkillSystem
{
    public class SkillUseManager : MonoBehaviour
    {
        public static SkillUseManager Instance;
        public Coroutine CurrentSkill;
        [SerializeField] private EffectPoolType _smileBombPoolType;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        public void Use(Skill skill)
        {
            if(skill.skillTime != 0)
                StartCoroutine(skill.SkillCooldown());
            switch (skill.modeType)
            {
                case ModeEnum.White:
                    StartCoroutine(Dash(skill));
                    break;
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

        private IEnumerator Dash(Skill skill)
        {
            Player player = transform.parent.GetComponent(typeof(Player)) as Player;
            SpriteRenderer spriteRenderer = transform.parent.Find("Visual").GetComponent<SpriteRenderer>();
            float dashDis = 2f;
            float dashDuration = 0.1f;
            Vector2 dashDir = new Vector2(Mathf.Sign(player.LookDirection.x) * dashDis, 0);
    
            LayerMask mask = LayerMask.GetMask("Default") | LayerMask.GetMask("Ground");
            Debug.DrawRay(player.transform.position + Vector3.up, dashDir*dashDis,Color.green );

            RaycastHit2D hit = Physics2D.Raycast(player.transform.position + Vector3.up, dashDir, dashDis, mask);

            Vector2 dashEndPos = dashDir - (Vector2)transform.position;
            
            if (hit.collider != null)
            {
                dashEndPos.x = hit.point.x;
            }
            
            player.transform.DOMoveX(dashEndPos.x, dashDuration);
            yield return new WaitForSeconds(dashDuration);
        }
        
        IEnumerator WaterBeam(Skill skill)
        {
            var dirvec = (gameObject.transform.position - _camera.ScreenToWorldPoint((Vector2)Input.mousePosition));
            float deg = Mathf.Atan2(dirvec.y, dirvec.x) * Mathf.Rad2Deg;
            var ef = gameObject.Pop(EffectPoolType.WaterBeam, gameObject.transform.position, Quaternion.identity);
            ef.gameObject.transform.parent = gameObject.transform;
            var caster = ef.gameObject.GetComponent<DamageCaster2D>();
            var o = SoundManager.Instance.PlaySFX("WaterBeam");
            while (Input.GetKey(KeyCode.Q) && skill.currentSkillGauge>0)
            {
                ef.gameObject.transform.rotation = Quaternion.Euler(0,0,deg+90);
                skill.doGaugeSkillCharge = false;
                caster.CastDamage(1);
                dirvec = (gameObject.transform.position - _camera.ScreenToWorldPoint((Vector2)Input.mousePosition));
                deg = Mathf.Atan2(dirvec.y, dirvec.x) * Mathf.Rad2Deg;
                yield return null;
                skill.currentSkillGauge-=Time.deltaTime;
            }
            ef.Push();
            skill.doGaugeSkillCharge = true;
            float elapsedTime = 0f;
            float volTemp = o.AudioSource.volume;
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                o.AudioSource.volume -= Time.deltaTime * volTemp;
                yield return null;
            }
            o.Push();
            o.AudioSource.volume = volTemp;

            
            yield break;
        }

        IEnumerator HappyBomb(Skill skill)
        {
            var bomb = gameObject.Pop(_smileBombPoolType, gameObject.transform.position + new Vector3(0,1f,0), Quaternion.identity) as SimplePoolingObject;
            var rb = bomb.gameObject.GetComponent<Rigidbody2D>();
            var col = bomb.gameObject.GetComponent<Collider2D>();
            skill.isUsingSkill = true;
            float elapsedTime = 0;
            col.isTrigger = true;
            yield return new WaitForSeconds(0.1f);
            while (!Input.GetKeyDown(KeyCode.Q) && !Input.GetMouseButtonDown(0) && elapsedTime < 8)
            {
                bomb.transform.position= gameObject.transform.position + new Vector3(0,1f,0);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            col.isTrigger = false;
            skill.isUsingSkill = false;
            rb.AddForce(((Vector2)(_camera.ScreenToWorldPoint((Vector2)Input.mousePosition) - gameObject.transform.position)).normalized * 7.5f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1.5f);
            bomb.Push();
            var ef = gameObject.Pop(EffectPoolType.SmileBumbExplosion, bomb.transform.position, Quaternion.identity);
            ef.gameObject.GetComponent<DamageCaster2D>().CastDamage(5);
            yield return new WaitForSeconds(0.3f);
            ef.Push();
        }

        private IEnumerator Spike(Skill skill)
        {
            skill.doGaugeSkillCharge = false;
            var ef = gameObject.Pop(EffectPoolType.SpikeEffect, gameObject.transform.position, Quaternion.identity);
            ef.gameObject.GetComponent<DamageCaster2D>().CastDamage(5);
            while ((Input.GetKey(KeyCode.Q) && skill.currentSkillGauge > 0.2))
            {
                ef.Push();
                ef = gameObject.Pop(EffectPoolType.SpikeEffect, gameObject.transform.position, Quaternion.identity);
                SoundManager.Instance.PlaySFX("Spike");
                yield return new WaitForSeconds(0.15f);
                ef.gameObject.GetComponent<DamageCaster2D>().CastDamage(5);
                yield return new WaitForSeconds(0.15f);
                skill.currentSkillGauge -= 0.3f;
            }
            skill.doGaugeSkillCharge = true;
        }

        private IEnumerator FireBall(Skill skill)
        {
            var dirvec = ((Vector2)(_camera.ScreenToWorldPoint((Vector2)Input.mousePosition) - gameObject.transform.position)).normalized;
            Debug.Log(dirvec);
            float deg = Mathf.Atan2(dirvec.y, dirvec.x) * Mathf.Rad2Deg;
            var ef = gameObject.Pop(EffectPoolType.FireBall, gameObject.transform.position, Quaternion.Euler(0,0,deg-90));
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
