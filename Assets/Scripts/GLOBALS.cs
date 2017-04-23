using System;
using UnityEngine;
using UnityEngine.Events;

public class EventPlayer : UnityEvent
{
    public EventPlayer()
    {
    }
}
public class EventGameState : UnityEvent { }
public class EventEnvironment : UnityEvent { }

public interface IUpgradeable
{
    void Upgrade();
    void Downgrade();
    int Level { get;}
}

public enum Level
{
    MAIN = 0,
    ENVIRONMENT = 1,
    PLAYER = 2,
    CREDITS = 3,
}