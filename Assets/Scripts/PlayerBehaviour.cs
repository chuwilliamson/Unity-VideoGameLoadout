using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    static public EventPlayer PlayerMovementEvent;
    public int Level
    {
        get { return _level; }
    }
    private int _level = 0;
    public Vector2 Position;
    void Awake()
    {
        PlayerMovementEvent = new EventPlayer();    
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
        ///lvl 1
        if(Level == 0)
        {
            var x = Position.x;
            var y = Position.y;
            if(Input.GetKeyDown(KeyCode.D))            
                x += 1;
            if(Input.GetKeyDown(KeyCode.A))
                x -= 1;
            if(Input.GetKeyDown(KeyCode.W))
                y += 1;
            if(Input.GetKeyDown(KeyCode.S))
                y -= 1;
            
            Position = new Vector2(x, y);
            
        }

        if(PlayerMovementEvent != null && oldpos != Position)
            PlayerMovementEvent.Invoke();
    }

    void DoMovement()
    {
        Debug.Log("moved");
    }
    
}
