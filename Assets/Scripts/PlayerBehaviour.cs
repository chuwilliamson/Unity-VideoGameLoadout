
using UnityEngine;
using UnityEngine.Events;
public class PlayerBehaviour : MonoBehaviour
{
    EventPlayer PlayerMovementEvent;
    public int Level;
    public Vector2 Position;
    void Awake()
    {
        PlayerMovementEvent = new EventPlayer();    
    }
    void Start()
    {
        Position = new Vector2(0, 0);
    }
    // Update is called once per frame
    void Update()
    {
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
        
    }
}
