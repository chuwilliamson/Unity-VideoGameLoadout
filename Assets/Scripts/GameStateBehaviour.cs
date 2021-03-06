﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameStateBehaviour : MonoBehaviour
{
    public static GameStateBehaviour Instance;

    public PlayerBehaviour playerBehaviour;

    public EnvironmentBehaviour environmentBehaviour;

    public GameBehaviour gameBehaviour;

    public Dictionary<string, IUpgradeable> upgradeDict;

    public List<IUpgradeable> Upgradeables;

    private List<GameObject> behaviours;

    [SerializeField]
    private GameObject HUD;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        var playerPrefab = Resources.Load("PlayerBehaviour");
        var gamePrefab = Resources.Load("GameBehaviour");
        var environmentPrefab = Resources.Load("EnvironmentBehaviour");

        var pb = Instantiate(playerPrefab, transform) as GameObject;
        var eb = Instantiate(environmentPrefab, transform) as GameObject;
        var gb = Instantiate(gamePrefab, transform) as GameObject;

        playerBehaviour = pb.GetComponent<PlayerBehaviour>();
        environmentBehaviour = eb.GetComponent<EnvironmentBehaviour>();
        gameBehaviour = gb.GetComponent<GameBehaviour>();



        playerBehaviour.PlayerMovementEvent.AddListener(OnPlayerMove);

        Upgradeables = new List<IUpgradeable>() { playerBehaviour, environmentBehaviour, gameBehaviour };

        behaviours = new List<GameObject>() { pb, eb, gb };
        behaviours.ForEach(go => go.SetActive(false));

        upgradeDict = new Dictionary<string, IUpgradeable>();
        Upgradeables.ForEach(u => upgradeDict.Add(u.GetType().ToString(), u));
    }

    public IUpgradeable GetUpgrade(string value)
    {
        IUpgradeable v;
        if(upgradeDict.TryGetValue(value, out v)) return v;
        return null;
    }

    public void OnPlayerMove()
    {
        var text = string.Format("{0}, {1}\n", playerBehaviour.Position.ToString(), 0);
        environmentBehaviour.DoText(text);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            for(int i = 0; i < HUD.transform.childCount; ++i)
            {
                var immediateChildGo = HUD.transform.GetChild(i).gameObject;
                immediateChildGo.SetActive(!immediateChildGo.activeSelf);
            }
        }
    }

    public void LoadScene(int index)
    {
        if(index == (int)Level.PLAYER)
        {
            environmentBehaviour.gameObject.SetActive(true);
            playerBehaviour.gameObject.SetActive(true);
            gameBehaviour.gameObject.SetActive(true);
        }
        else
        {
            environmentBehaviour.gameObject.SetActive(false);
            playerBehaviour.gameObject.SetActive(false);
            gameBehaviour.gameObject.SetActive(false);
        }

        SceneManager.LoadScene(index);
    }
}