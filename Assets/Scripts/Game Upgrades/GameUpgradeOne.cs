using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUpgradeOne : MonoBehaviour, IGameUpgrade
{
    public void VicotryCondition()
    {
        if (GetComponent<UpgradableGame>().Player.Position == GetComponent<UpgradableGame>().Environment.Exit.Position)
        {
            SceneManager.LoadScene(3);
        }
    }
}
