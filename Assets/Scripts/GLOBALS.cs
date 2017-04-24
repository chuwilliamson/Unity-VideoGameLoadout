using System;
using UnityEngine;
using UnityEngine.Events;

public class EventPlayer : UnityEvent { }
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
    Main = 0,
    Environment = 1,
    Player = 2,
    Credits = 3,
}