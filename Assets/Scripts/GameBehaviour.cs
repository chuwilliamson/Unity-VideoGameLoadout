using System;
using UnityEngine;

public class GameBehaviour : MonoBehaviour, IUpgradeable
{
    public int Level
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public void Downgrade()
    {
        throw new NotImplementedException();
    }

    public void Upgrade()
    {
        throw new NotImplementedException();
    }
}