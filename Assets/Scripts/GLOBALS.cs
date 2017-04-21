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


