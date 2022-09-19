using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera gameCamera;
    [SerializeField] private CinemachineVirtualCamera successCamera;
    [SerializeField] private CinemachineVirtualCamera failCamera;

    private void Awake()
    {
        SelectGameCamera();
    }

    private void OnEnable()
    {
        EventManager.LevelSuccesEvent += SelectSuccessCamera;
        EventManager.LevelFailEvent += SelectFailCamera;
    }

    private void OnDisable()
    {
        EventManager.LevelSuccesEvent -= SelectSuccessCamera;
        EventManager.LevelFailEvent -= SelectFailCamera;
    }

    private void SelectGameCamera()
    {
        gameCamera.Priority = 1;

        successCamera.Priority = 0;
        failCamera.Priority = 0;
    }

    private void SelectSuccessCamera()
    {
        successCamera.Priority = 1;

        gameCamera.Priority = 0;
        failCamera.Priority = 0;
    }

    private void SelectFailCamera()
    {
        failCamera.Priority = 1;

        gameCamera.Priority = 0;
        successCamera.Priority = 0;
    }
}
