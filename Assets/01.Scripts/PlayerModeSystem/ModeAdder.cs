using _01.Scripts.SkillSystem;
using UnityEngine;

namespace _01.Scripts.PlayerModeSystem
{
    public class ModeAdder : MonoBehaviour
    {
        [SerializeField] private static ModeEnum reward;
        public bool isUsed = false;
        public void Trigger()
        {
            if (isUsed) return;
            global::_01.Scripts.PlayerModeSystem.PlayerModeSystem.Instance.AddMode(reward);
            isUsed = true;
        }
    }
}
