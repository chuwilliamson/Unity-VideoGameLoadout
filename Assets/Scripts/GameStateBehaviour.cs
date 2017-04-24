using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameStateBehaviour : MonoBehaviour
{
    public static GameStateBehaviour Instance;

    public EventGameState onUpgradeChange = new EventGameState();

    public PlayerBehaviour playerBehaviour;

    public EnvironmentBehaviour environmentBehaviour;

    public GameBehaviour gameBehaviour;

    public Dictionary<string, IUpgradeable> UpgradeDict = new Dictionary<string, IUpgradeable>();

    public List<IUpgradeable> Upgradeables = new List<IUpgradeable>();

    private List<GameObject> behaviours = new List<GameObject>();

    [SerializeField] private GameObject HUD;

    bool initializing;

    void Awake()
    {
        initializing = true;
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
        var gamePrefab = Resources.Load("GameBehaviour");
        var environmentPrefab = Resources.Load("EnvironmentBehaviour");

        var pb = Instantiate(playerPrefab, transform) as GameObject;
        var eb = Instantiate(environmentPrefab, transform) as GameObject;
        var gb = Instantiate(gamePrefab, transform) as GameObject;

        playerBehaviour = pb.GetComponent<PlayerBehaviour>();
        environmentBehaviour = eb.GetComponent<EnvironmentBehaviour>();
        gameBehaviour = gb.GetComponent<GameBehaviour>();

        playerBehaviour.PlayerMovementEvent.AddListener(OnPlayerMove);

        behaviours = new List<GameObject>() {pb, eb, gb};

        behaviours.ForEach(go => go.SetActive(false));
        initializing = false;
    }

    public IUpgradeable GetUpgrade(string value)
    {
        IUpgradeable v;
        if (UpgradeDict.TryGetValue(value, out v)) return v;
        return null;
    }

    public void OnPlayerMove()
    {
        var envinfo = FindObjectOfType<Grid>().GetNode(playerBehaviour.Position).Dialogue;
        var text = string.Format("{0}, {1}\n", playerBehaviour.Position, envinfo);
        environmentBehaviour.DoText(text);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = 0; i < HUD.transform.childCount; ++i)
            {
                var immediateChildGo = HUD.transform.GetChild(i).gameObject;
                immediateChildGo.SetActive(!immediateChildGo.activeSelf);
            }
        }
    }

    public void UnregisterUpgrade(IUpgradeable upgrade)
    {
        var key = upgrade.GetType().ToString();

        if (!UpgradeDict.ContainsKey(key))
        {
            Debug.Log("can not remove a key that isn't in the dictionary");
            return;
        }

        UpgradeDict.Remove(key);
        Upgradeables.Remove(upgrade);
        Debug.Log("UnRegister " + key);
        Debug.Log("dictionary @ " + UpgradeDict.Count);
        onUpgradeChange.Invoke();
    }

    public void RegisterUpgrade(IUpgradeable upgrade)
    {
        var key = upgrade.GetType().ToString();

        if (UpgradeDict.ContainsKey(key))
        {
            Debug.LogWarning("Tried to add upgrade already present in dictionary aborting...");
            return;
        }
        Upgradeables.Add(upgrade);
        UpgradeDict.Add(upgrade.GetType().ToString(), upgrade);
        onUpgradeChange.Invoke();
        Debug.Log("Register " + key);
        Debug.Log("dictionary @ " + UpgradeDict.Count);
    }

    public int UpgradeCount
    {
        get { return Upgradeables.Count; }
    }

    public void LoadScene(int index)
    {
        if (initializing)
            return;
        if (index == (int) Level.Player)
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