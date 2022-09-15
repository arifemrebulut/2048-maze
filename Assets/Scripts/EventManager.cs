using System;
using UnityEngine;

public static class EventManager
{
    // GameCycle Events
    public static Action GameStartEvent;
    public static Action LevelFailEvent;
    public static Action LevelSuccesEvent;

    public static void CallGameStartEvent() => GameStartEvent?.Invoke();
    public static void CallLevelFailEvent() => LevelFailEvent?.Invoke();
    public static void CallLevelSuccessEvent() => LevelSuccesEvent?.Invoke();

    // Player Events
    public static Action MergeNumbersEvent;
    public static Action<Vector3> MoveEvent;

    public static void CallMergeNumbersEvent() => MergeNumbersEvent?.Invoke();
    public static void CallMoveEvent(Vector3 direction) => MoveEvent?.Invoke(direction);
}

