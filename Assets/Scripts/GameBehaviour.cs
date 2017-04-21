using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour, IUpgradeable
{
    private int _level;
    // Use this for initialization
    void Start()
    {
        _level = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Upgrade()
    {
        _level = (_level < 3) ? ++_level : 3;


    }

    public void Downgrade()
    {
        _level = (_level > 0) ? --_level : 0;
    }

    public int Level { get { return _level; } }
}