using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradableGame : MonoBehaviour
{
    public PlayerBehaviour Player;
    public Grid Environment;

    List<string> UpgradeNames;

    public IGameUpgrade CurrentUpgrade;

    public void Upgrade(IGameUpgrade upgrade)
    {
        if (upgrade != CurrentUpgrade)
            CurrentUpgrade = upgrade;
    }

    void Update()
    {
        if (CurrentUpgrade == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            CurrentUpgrade.VicotryCondition();
        }
    }
}
