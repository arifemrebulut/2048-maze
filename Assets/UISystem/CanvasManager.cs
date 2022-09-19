using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIControllerType
{
    GameUI,
    SuccessUI,
    FailUI
}

public class CanvasManager : MonoBehaviour
{
    List<IUIController> canvasControllerList;
    IUIController lastActiveCanvas;

    private void OnEnable()
    {
        EventManager.LevelPlaying += GameUI;
        EventManager.LevelSuccesEvent += SuccessUI;
        EventManager.LevelFailEvent += FailUI;
    }

    private void OnDisable()
    {
        EventManager.LevelPlaying -= GameUI;
        EventManager.LevelSuccesEvent -= SuccessUI;
        EventManager.LevelFailEvent -= FailUI;
    }

    private void Awake()
    {
        canvasControllerList = GetComponentsInChildren<IUIController>().ToList();
        canvasControllerList.ForEach(x => x.Close());
        SwitchCanvas(UIControllerType.GameUI);
    }

    private void GameUI()
    {
        SwitchCanvas(UIControllerType.GameUI);
    }

    private void SuccessUI()
    {
        SwitchCanvas(UIControllerType.SuccessUI);
    }

    private void FailUI()
    {
        SwitchCanvas(UIControllerType.FailUI);
    }

    private void SwitchCanvas(UIControllerType _type)
    {
        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.Close();
        }

        IUIController desiredCanvas = canvasControllerList.Find(x => x.UIControllerType == _type);
        if (desiredCanvas != null)
        {
            desiredCanvas.Open();
            lastActiveCanvas = desiredCanvas;
        }
        else { Debug.LogWarning("The desired canvas was not found!"); }
    }

    public void CallSuccess()
    {
        EventManager.CallLevelSuccessEvent();
    }

    public void CallFail()
    {
        EventManager.CallLevelFailEvent();
    }
}