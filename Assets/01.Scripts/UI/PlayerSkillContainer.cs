using System;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts.UI
{
    public class PlayerSkillContainer : MonoBehaviour
    {
        private PlayerModeSystem.PlayerModeSystem _playerModeSystem;
        [SerializeField] private Image skillIcon;
        [SerializeField] private Image skillFill;
        [SerializeField] private Image playerIcon;
        [SerializeField] private Image nextIcon;
        [SerializeField] private Image nextArrow;

        private void Start()
        {
            _playerModeSystem = FindFirstObjectByType<Player>().GetComponent<PlayerModeSystem.PlayerModeSystem>();
            _playerModeSystem.onModeChanged.AddListener(ChangeSkill);
            _playerModeSystem.onModeAdded.AddListener(AddMode);        
        }


        private void ChangeSkill()
        {
            skillIcon.sprite = _playerModeSystem.currentMode.skillInstance.skillSprite;
            skillFill.fillAmount = _playerModeSystem.currentMode.skillInstance.GetSkillCoolPercent();
            playerIcon.color = _playerModeSystem.currentMode.modeInfo.modeColor;
            if (_playerModeSystem.nextMode is not null)
            {
                nextIcon.gameObject.SetActive(true);
                nextIcon.color = _playerModeSystem.nextMode.modeInfo.modeColor;
            }
            else
            {
                nextIcon.gameObject.SetActive(false);
                nextArrow.gameObject.SetActive(false);
            }
        }

        private void AddMode()
        {
            if (_playerModeSystem.ModeLength == 1)
            {
                nextIcon.enabled = true;
            }
            skillIcon.sprite = _playerModeSystem.currentMode.skillInstance.skillSprite;
            skillFill.fillAmount = _playerModeSystem.currentMode.skillInstance.GetSkillCoolPercent();
            playerIcon.color = _playerModeSystem.currentMode.modeInfo.modeColor;
            nextIcon.color = _playerModeSystem.nextMode.modeInfo.modeColor;
        }

        private void Update()
        {
            skillFill.fillAmount = 1 - _playerModeSystem.currentMode.skillInstance.GetSkillCoolPercent();
        }
    }
}
