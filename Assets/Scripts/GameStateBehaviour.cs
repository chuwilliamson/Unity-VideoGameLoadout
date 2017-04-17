using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStateBehaviour : MonoBehaviour
{
    public static GameStateBehaviour Instance;   

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
    }

    void Start()
    {
        var playerPrefab  = Resources.Load("PlayerBehaviour");
        var gamePrefab = Resources.Load("GameBehaviour");
        var environmentPrefab = Resources.Load("EnvironmentBehaviour");
        var pb = Instantiate(playerPrefab) as GameObject;
        pb.transform.SetParent(transform);
        var eb = Instantiate(environmentPrefab) as GameObject;
        var gb = Instantiate(gamePrefab) as GameObject;
        eb.transform.SetParent(transform);
        gb.transform.SetParent(transform);
        playerBehaviour = pb.GetComponent<PlayerBehaviour>();
        PlayerBehaviour.PlayerMovementEvent.AddListener(OnPlayerMove);
        environmentBehaviour = eb.GetComponent<EnvironmentBehaviour>();
        environmentBehaviour.gameObject.SetActive(false);
        gameBehaviour = gb.GetComponent<GameBehaviour>();
    }

    public PlayerBehaviour playerBehaviour;
    public EnvironmentBehaviour environmentBehaviour;
    public GameBehaviour gameBehaviour;
    
    public void OnPlayerMove()
    {
        environmentBehaviour.DoText();
        if(playerBehaviour.Position == new Vector2(2, 2))        
            LoadScene(3);
        
    }
    public void LoadScene(int index)
    {
        if(index == 2)
            environmentBehaviour.gameObject.SetActive(true);
        SceneManager.LoadScene(index);
    }
}
