using System;
using UnityEngine;

public class GameBehaviour : MonoBehaviour, IUpgradeable
{
    private UpgradableGame m_instance;

    public int GameLevel;
    public int Level{ get { return m_instance.Level; } }
 
    public void Awake()
    {
        
        var obj = Resources.Load("UpgradeableGame");
        var ug = Instantiate(obj, transform) as GameObject;
        m_instance = ug.GetComponent<UpgradableGame>();
    }

    public void Start()
    {
        GameLevel = Level;
    }

    public void Downgrade()
    {
        m_instance.Downgrade();
    }

    public void Upgrade()
    {
        m_instance.Upgrade();
    }
}