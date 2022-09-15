using System;

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

    public static void CallMergeNumbersEvent() => MergeNumbersEvent?.Invoke();
}

