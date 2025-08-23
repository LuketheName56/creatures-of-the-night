using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Character character;
    [SerializeField] private CinemachineCamera defaultCamera;
    
    [Header("Damping Settings")]
    [SerializeField] private float fallVelocityThreshold = -15f;
    [SerializeField] private float fallDamping = 0.25f;
    [SerializeField] private float dampingTransitionDuration = 0.35f;
    
    private CinemachineCamera _activeCamera;
    private CinemachinePositionComposer _positionComposer;
    private float _defaultDamping;
    private bool _isFalling;
    private bool _isDampingTransitioning;
    
    private void Awake()
    {
        _activeCamera = defaultCamera;
        _positionComposer = _activeCamera.GetComponent<CinemachinePositionComposer>();
        _defaultDamping = _positionComposer.Damping.y;
    }

    private void Update()
    {
        if (_isDampingTransitioning) return;
        
        bool shouldApplyFallDamping = character.VerticalVelocity < fallVelocityThreshold && !_isFalling;
        
        if (shouldApplyFallDamping) 
        {
            _isFalling = true;
            TransitionDamping(fallDamping);
        }
        else if (character.VerticalVelocity >= 0 && _isFalling) 
        {
            _isFalling = false;
            TransitionDamping(_defaultDamping);
        }
    }
    
    private void TransitionDamping(float targetDamping)
    {
        LeanTween.value(_positionComposer.Damping.y, targetDamping, dampingTransitionDuration)
            .setOnStart(() => _isDampingTransitioning = true)
            .setOnComplete(() => _isDampingTransitioning = false)
            .setOnUpdate(val => _positionComposer.Damping.y = val);
    }

    public void SwitchCamera(CinemachineCamera newCamera)
    {
        _activeCamera.gameObject.SetActive(false);
        newCamera.gameObject.SetActive(true);
        _activeCamera = newCamera;
    }

    public void RevertToDefaultCamera()
    {
        _activeCamera.gameObject.SetActive(false);
        defaultCamera.gameObject.SetActive(true);
        _activeCamera = defaultCamera;
    }
}