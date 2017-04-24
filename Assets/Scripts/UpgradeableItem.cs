using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeableItem : MonoBehaviour, IUpgradeable {
    
    public void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public void Downgrade()
    {
        throw new System.NotImplementedException();
    }

    int m_level;
    public int Level { get; private set; }
}
