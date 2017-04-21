using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour, IUpgradeable
{
    // Use this for initialization
    void Start()
    {
        m_level = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private int m_level;

    public void Upgrade()
    {
        m_level++;
    }

    public void Downgrade()
    {
        m_level--;
    }

    public int Level
    {
        get { return m_level; }
    }
}