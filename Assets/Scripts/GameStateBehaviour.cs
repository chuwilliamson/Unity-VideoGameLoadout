using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameStateBehaviour : MonoBehaviour
{
    public static GameStateBehaviour Instance;
    public PlayerBehaviour playerBehaviour;
    public EnvironmentBehaviour environmentBehaviour;
    public GameBehaviour gameBehaviour;
    public Dictionary<string, IUpgradeable> upgradeDict;
    
    public List<IUpgradeable> Upgradeables;
    void Awake()
    {
        
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    
        var playerPrefab = Resources.Load("PlayerBehaviour");
        var gamePrefab = Resources.Load("UpgradeableGame");
        var environmentPrefab = Resources.Load("EnvironmentBehaviour");

        var pb = Instantiate(playerPrefab, transform) as GameObject;
        var eb = Instantiate(environmentPrefab,transform) as GameObject;
        var gb = Instantiate(gamePrefab, transform) as GameObject;

        playerBehaviour = pb.GetComponent<PlayerBehaviour>();
        PlayerBehaviour.PlayerMovementEvent.AddListener(OnPlayerMove);

        environmentBehaviour = eb.GetComponent<EnvironmentBehaviour>();
        gameBehaviour = gb.GetComponent<GameBehaviour>();

        
        environmentBehaviour.gameObject.SetActive(false);
        playerBehaviour.gameObject.SetActive(false);
        gameBehaviour.gameObject.SetActive(false);

        Upgradeables = new List<IUpgradeable>() {playerBehaviour, environmentBehaviour, gameBehaviour};
        upgradeDict = new Dictionary<string, IUpgradeable>();
        Upgradeables.ForEach(u => upgradeDict.Add(u.GetType().ToString(), u));
        foreach (var kvp in upgradeDict)
        {
            print(kvp.Key);
        }

    }

    public void GetUpgrade(string value)
    {
        IUpgradeable v;
        if (upgradeDict.TryGetValue(value, out v))
        {
            print(v);
        }

    }

    public void OnPlayerMove()
    {
        environmentBehaviour.DoText();
    }

    public void LoadScene(int index)
    {
        if(index == 2)
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