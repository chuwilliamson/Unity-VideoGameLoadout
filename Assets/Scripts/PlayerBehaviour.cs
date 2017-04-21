using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, IUpgradeable
{
    public static EventPlayer PlayerMovementEvent;

    public Vector2 Position;

    int _level = 0;

    public void Upgrade()
    {
        _level = (_level < 3) ? ++_level : 3;
    }

    public void Downgrade()
    {
        _level = (_level > 0) ? --_level : 0;
    }

    public int Level
    {
        get { return _level; }
    }

    void Awake()
    {
        PlayerMovementEvent = new EventPlayer();
    }

    private void DoMovement()
    {
        Debug.Log("moved");
    }

    void OnEnable()
    {
        Position = new Vector2(0, 0);
    }

    void OnDisable()
    {

        Position = new Vector2(0, 0);
    } 
    void Start()
    {
        PlayerMovementEvent.AddListener(DoMovement);
        Position = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        var oldpos = Position;
        if(Level < 1)
            return;
        var x = Position.x;
        var y = Position.y;
        if (Level == 1)
        {
            if (Input.GetKeyDown(KeyCode.D)) x += 1;
            if (Input.GetKeyDown(KeyCode.A)) x -= 1;
            if (Input.GetKeyDown(KeyCode.W)) y += 1;
            if (Input.GetKeyDown(KeyCode.S)) y -= 1;
        }
         
        Position = new Vector2(x, y);
        if ((PlayerMovementEvent != null) && (oldpos != Position))
            PlayerMovementEvent.Invoke();
    }

    public void MovePlayer(string dir)
    {
        if(dir.Length > 1)
            return;
        Debug.Log("move player " + dir);
        var oldpos = Position;

        var x = 0;
        var y = 0;

        if(dir == "d") x += 1;
        if(dir == "a") x -= 1;
        if(dir == "w") y += 1;
        if(dir == "s") y -= 1;

        Position = new Vector2(x, y);

        if((PlayerMovementEvent != null) && oldpos != Position)
            PlayerMovementEvent.Invoke();

    }
}