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
    public string Dialogue; 

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
        GenerateInformation();
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

    public void GenerateInformation()
    {
        Dialogue = "";
        foreach(var neighbor in this.Neighbors)
        {
            if (neighbor.Habitant == Habitants.WUMPUS)
                Dialogue += "Smells";
            if (neighbor.Habitant == Habitants.GOLD)
                Dialogue += "Bright";
            if (neighbor.Habitant == Habitants.PIT)
                Dialogue += "Breezy";
        }        
    }

    public void changeColor(Color col)
    {
        GetComponent<Renderer>().material.color = col;
    }

    string NodeInformation()
    {
        return this.Position.ToString() + " " + Dialogue;
    }
}


