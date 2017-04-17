using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Node : MonoBehaviour
{    
    public enum Habitants
    {
        WUMPUS,
        GOLD, 
        PIT,
        NONE
    }
    public Habitants Habitant; //What is in the node
    public Vector2 Position; //Location of the node
    public List<Node> Neighbors; //List of neighboring nodes    

    void Awake()
    {
        Neighbors = new List<Node>();
        Habitant = Habitants.NONE;
    }    

    public void GetNeighbors(Grid graph)
    {
        List<Vector2> ValidNeighborPositions = new List<Vector2>()
        {
            this.Position + new Vector2(0, 1),
            this.Position + new Vector2(0, -1),
            this.Position + new Vector2(-1, 0),
            this.Position + new Vector2(1, 0)
        };

        foreach(var node in graph.Nodes)
        {
            foreach(var position in ValidNeighborPositions)
            {
                if(node.Position == position)
                {
                    Neighbors.Add(node);
                }
            }
        }
    }

    void Update()
    {
        if(Habitant == Habitants.NONE)
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
        if (Habitant == Habitants.GOLD)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        if (Habitant == Habitants.PIT)
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
        if (Habitant == Habitants.WUMPUS)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public List<string> GetInformation()
    {
        List<string> Context = new List<string>();
        foreach(var neighbor in this.Neighbors)
        {
            if (neighbor.Habitant == Habitants.WUMPUS)
                Context.Add("Smells");
            if (neighbor.Habitant == Habitants.GOLD)
                Context.Add("Bright");
            if (neighbor.Habitant == Habitants.PIT)
                Context.Add("Breezy");            
        }
        return Context;
    }

    public void changeColor(Color col)
    {
        GetComponent<Renderer>().material.color = col;
    }
}


