using UnityEngine;

namespace _01.Scripts.SkillSystem
{
    public class SkillInfoManager : MonoBehaviour
    {
        public static SkillInfoManager Instance ;
        
        //스킬 에디터
        public Skill[] skills;
        [HideInInspector] public bool[] toggled;
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);
        }

        public Skill GetSkill(ModeEnum mode)
        {
            var o = new Skill();
            o.Init(skills[(int)mode]);
            return o;
        }
    }
}
