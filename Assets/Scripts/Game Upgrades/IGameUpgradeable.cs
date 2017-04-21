using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGameUpgrade
{
    void VicotryCondition(PlayerBehaviour player);

    void GameUpdate(PlayerBehaviour player);
}
