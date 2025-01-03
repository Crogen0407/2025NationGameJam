using _01.Scripts.SkillSystem;
using UnityEngine;

namespace _01.Scripts.PlayerModeSystem
{
    public class ModeManager : MonoBehaviour
    {
        public static ModeManager Instance;
    
        //모드 에디터
        public PlayerMode[] PlayerModes;
        [HideInInspector] public bool[] toggled;

        private void Awake()
        {
            Instance = this;
        }

        public PlayerMode GetMode(ModeEnum modeEnum)
        {
            var mode = new PlayerMode
            {
                modeInfo = PlayerModes[(int)modeEnum].modeInfo
            };
            mode.Init();
            return mode;
        }
    }
}
