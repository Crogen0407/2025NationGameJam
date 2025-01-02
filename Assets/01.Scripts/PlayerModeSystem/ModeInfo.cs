using _01.Scripts.SkillSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Scripts.PlayerModeSystem
{
    [CreateAssetMenu(fileName = "Mode Info", menuName = "Scriptable Object/Mode Info", order = 31)]
    public class ModeInfo : ScriptableObject
    {
        [HideInInspector] public ModeEnum mode;
        public Sprite playerIcon;
        public Color modeColor;
    }
}
