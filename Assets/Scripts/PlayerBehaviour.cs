using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehaviour : MonoBehaviour, IUpgradeable
{
    public EventPlayer PlayerMovementEvent;

    public Vector2 Position;

    public TMP_InputField TMP_ChatInput;

    [SerializeField]
    int _level = 0;

    void Awake()
    {
        PlayerMovementEvent = new EventPlayer();
    }

    void OnEnable()
    {
        Position = new Vector2(0, 0);
        TMP_ChatInput.onSubmit.AddListener(MovePlayer);
    }

    void OnDisable()
    {
        Position = new Vector2(0, 0);
        TMP_ChatInput.onSubmit.RemoveListener(MovePlayer);
    }

    void Start()
    {
        Position = new Vector2(0, 0);
        TMP_ChatInput.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        var oldpos = Position;
        if (Level < 1) return;
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
        if ((PlayerMovementEvent != null) && (oldpos != Position)) PlayerMovementEvent.Invoke();
    }
    
    private void MovePlayer(string dir)
    {
        TMP_ChatInput.ActivateInputField();    
        TMP_ChatInput.text = string.Empty;
        if (dir.Length > 1) return;

        var oldpos = Position;

        var x = oldpos.x;
        var y = oldpos.y;

        if (dir == "d") x += 1;
        if (dir == "a") x -= 1;
        if (dir == "w") y += 1;
        if (dir == "s") y -= 1;

        Position = new Vector2(x, y);

        if ((PlayerMovementEvent != null) && oldpos != Position) PlayerMovementEvent.Invoke();
    }

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
        get
        {
            return _level;
        }
    }
}