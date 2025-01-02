using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Scripts.SkillSystem
{
    public class SkillInfoManager : MonoBehaviour
    {
        public static SkillInfoManager Instance;
        public Animator animator;

        
        //스킬 에디터
        public Skill[] skills;
        [HideInInspector] public bool[] toggled;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public Skill GetSkill(SkillEnum skill)
        {
            var o = new Skill();
            o.Init(skills[(int)skill]);
            return o;
        }
    }
}
