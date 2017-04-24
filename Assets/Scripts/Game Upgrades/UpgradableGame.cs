using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradableGame : MonoBehaviour, IUpgradeable
{
    public Grid Environment;

    public IGameUpgrade CurrentUpgrade;

    public List<MonoBehaviour> Upgrades;

    public int Level
    {
        get
        {
            return Upgrades.IndexOf(CurrentUpgrade as MonoBehaviour);
        }
    }

    private void Awake()
    {
        ValidateUpgrades();
        CurrentUpgrade = Upgrades[0] as IGameUpgrade;
    }

    public void Upgrade()
    {
        if (Upgrades.IndexOf(CurrentUpgrade as MonoBehaviour) != Upgrades.Count - 1)
            CurrentUpgrade = Upgrades[Upgrades.IndexOf(CurrentUpgrade as MonoBehaviour) + 1] as IGameUpgrade;
        else
            SceneManager.LoadScene(3);
        Debug.Log(CurrentUpgrade.GetType());
    }

    void ValidateUpgrades()
    {
        var validUpgrades = new List<MonoBehaviour>();
        for(var i = 0; i < Upgrades.Count; i++)
        {
            if (Upgrades[i] is IGameUpgrade)
                validUpgrades.Add(Upgrades[i]);
            else            
                continue;            
        }
        Upgrades = validUpgrades;
    }

    void Update()
    {
        if (CurrentUpgrade == null)
        {
            GameStateBehaviour.Instance.LoadScene(0);
        }
        else
        {
            CurrentUpgrade.GameUpdate();
        }
    }

    public void Downgrade()
    {
        if (Upgrades.IndexOf(CurrentUpgrade as MonoBehaviour) != 0)
            CurrentUpgrade = Upgrades[Upgrades.IndexOf(CurrentUpgrade as MonoBehaviour) - 1] as IGameUpgrade;
        else
            SceneManager.LoadScene(0);
    }
}
