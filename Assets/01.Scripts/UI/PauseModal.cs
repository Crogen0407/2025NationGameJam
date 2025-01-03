using System;
using System.Collections;
using System.Collections.Generic;
using Crogen.PowerfulInput;
using DG.Tweening;
using TMPro;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseModal : MonoBehaviour
{
   [SerializeField] private GameObject backGround;
   [SerializeField] private GameObject panel;
   [SerializeField] private Animator anim;
   [SerializeField] private Slider bgmVolumeSlider;
   [SerializeField] private Slider sfxVolumeSlider;
   [SerializeField] private Slider frameSlider;
   [SerializeField] private TextMeshProUGUI maxFrameText;
   [SerializeField] private AudioMixer audioMixer;
   private bool _isUiOn = false;
   

   private void Awake()
   {
      DontDestroyOnLoad(gameObject);
      backGround.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 0f);
      panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-3000f, 0f);
      bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChange);
      sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChange);
      frameSlider.onValueChanged.AddListener(OnFrameChange);
      maxFrameText.text = ((int)frameSlider.value * 15).ToString();
   }

   private void Update()
   {
      if(Input.GetKeyDown(KeyCode.Escape))
      {
         if (_isUiOn)
            PauseOff();
         else
            PauseOn();
         
      }
   }

   private void OnBgmVolumeChange(float volume)
   {
      audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
   }

   private void OnSfxVolumeChange(float volume)
   {
      audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
   }

   private void OnFrameChange(float frame)
   {
      Application.targetFrameRate = (int)frame * 15;
      maxFrameText.text = ((int)frame * 15).ToString();
   }

   public void OnExit()
   {
      Application.Quit();
   }

   public void OnBackToTheScene()
   {
      PauseOff();
   }

   public void PauseOn()
   {
      backGround.SetActive(true);
      _isUiOn = true;
      backGround.GetComponent<Image>().DOFade(0.4f, 1f).SetUpdate(true);
      panel.GetComponent<RectTransform>().DOAnchorPosX(0, 1f).SetUpdate(true);  
   }

   public void PauseOff()
   {
      _isUiOn = false;
      backGround.GetComponent<Image>().DOFade(0f, 1f).OnComplete(()=>backGround.SetActive(false)).SetUpdate(true);
      panel.GetComponent<RectTransform>().DOAnchorPosX(-3000f, 1f).SetUpdate(true);
   }
   
}
