using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradableGame : MonoBehaviour
{
    public PlayerBehaviour Player;
    public Grid Environment;

    public IGameUpgrade CurrentUpgrade;

    public List<MonoBehaviour> Upgrades;

    private void Awake()
    {
        ValidateUpgrades();
    }

    public void Upgrade(IGameUpgrade upgrade)
    {
        if (upgrade != CurrentUpgrade)
        {
            CurrentUpgrade = upgrade;
        }
    }

    void ValidateUpgrades()
    {
        var validUpgrades = new List<MonoBehaviour>();
        for(var i = 0; i < Upgrades.Count; i++)
        {
            if (Upgrades[i] is IGameUpgrade)
                validUpgrades.Add(Upgrades[i]);
            else
            {
                continue;
            }
        }
        Upgrades = validUpgrades;
    }

    void Update()
    {
        if (CurrentUpgrade == null)
        {
            //SceneManager.LoadScene(0);
        }
        else
        {
            CurrentUpgrade.GameUpdate(Player);
        }
    }
}
