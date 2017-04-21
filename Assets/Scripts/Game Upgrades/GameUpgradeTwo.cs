using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUpgradeTwo : MonoBehaviour, IGameUpgrade
{
    public void VicotryCondition(PlayerBehaviour player)
    {
        if (GetComponent<UpgradableGame>().Player.Position == GetComponent<UpgradableGame>().Environment.Exit.Position)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void GameUpdate(PlayerBehaviour player)
    {
        VicotryCondition(player);
    }
}
