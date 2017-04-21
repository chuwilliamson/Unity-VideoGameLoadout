using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public static EventPlayer PlayerMovementEvent;

    public Vector2 Position;

    public PlayerBehaviour()
    {
        Level = 0;
    }

    public int Level { get; private set; }

    private void Awake()
    {
        PlayerMovementEvent = new EventPlayer();
    }

    private void DoMovement()
    {
        Debug.Log("moved");
    }

    private void OnEnable()
    {
        Position = new Vector2(0, 0);
    }
    private void Start()
    {
        PlayerMovementEvent.AddListener(DoMovement);
        Position = new Vector2(0, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        var oldpos = Position;

        if (Level == 0)
        {
            var x = Position.x;
            var y = Position.y;
            if (Input.GetKeyDown(KeyCode.D)) x += 1;
            if (Input.GetKeyDown(KeyCode.A)) x -= 1;
            if (Input.GetKeyDown(KeyCode.W)) y += 1;
            if (Input.GetKeyDown(KeyCode.S)) y -= 1;

            Position = new Vector2(x, y);
        }

        if ((PlayerMovementEvent != null) && (oldpos != Position)) PlayerMovementEvent.Invoke();
    }
}