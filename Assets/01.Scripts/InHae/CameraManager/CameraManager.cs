using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _cameraEventChannel;
    [SerializeField] private GameEventChannelSO _systemEventChannel;
    [SerializeField] private string _nextSceneName;
    
    private CinemachineVirtualCamera _vCam;
    private bool _isZoomIn;

    private void Awake()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
        
        _cameraEventChannel.AddListener<CameraZoomInEvent>(HandleCameraZoomIn);
    }

    private void OnDestroy()
    {
        _cameraEventChannel.RemoveListener<CameraZoomInEvent>(HandleCameraZoomIn);
    }

    private void HandleCameraZoomIn(CameraZoomInEvent evt)
    {
        if(_isZoomIn)
            return;

        _isZoomIn = true;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(evt.targetPos, evt.moveTime));
        sequence.AppendInterval(0.7f);
        sequence.Append(DOTween.To(() => _vCam.m_Lens.OrthographicSize, f => _vCam.m_Lens.OrthographicSize = f,
            evt.lensSize, evt.zoomInTime));
        sequence.OnComplete(() =>
        {
            FadeScreenEvent fadeEvt = SystemEvents.FadeScreenEvent;
            fadeEvt.isFadeIn = true;

            _systemEventChannel.AddListener<FadeComplete>(HandleFadeComplete);
            _systemEventChannel.RaiseEvent(fadeEvt);
            _isZoomIn = false;
        });
        
        
    }

    private void HandleFadeComplete(FadeComplete obj)
    {
        _systemEventChannel.RemoveListener<FadeComplete>(HandleFadeComplete);
        SceneManager.LoadScene(_nextSceneName);
    }
}
