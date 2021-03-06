﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUpgradeThree : MonoBehaviour, IGameUpgrade
{
    public void VictoryCondition()
    {
        if (GameStateBehaviour.Instance.playerBehaviour.Position == GetComponent<UpgradableGame>().Environment.Exit.Position)
        {
            GetComponent<UpgradableGame>().Environment.isRandom = true;
            GameStateBehaviour.Instance.LoadScene(0);
        }
    }

    public void GameUpdate()
    {
        VictoryCondition();
    }
}
