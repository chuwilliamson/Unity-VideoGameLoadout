﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// How to use
/// ===============
/// To get a specific node in the grid you will need to use
/// the GetNode funciton and give it a Vector 2 if a node is found 
/// in the grid with the same position value as the vector passed
/// in it will return that node. Otherwise it will return null
/// To set up a new static grid you can invoke the SerGridValues fucntion
/// and pass in a dictionary where the key is a vecotr representing a nodes
/// position and the value is a string specifying what you are trying to said 
/// the node as.
/// ex. 
///          Dictionary<Vector2, string> Test = new Dictionary<Vector2, string>();
///          Test.Add(new Vector2(0, 0), "start"); Looks for a node at <0,0> and wants to make it the start node
///          Test.Add(new Vector2(2, 0), "pit"); Looks for a node at <2,0> and wants to make it a pit
///          Test.Add(new Vector2(2, 2), "pit");
///          Test.Add(new Vector2(3, 3), "pit");
///          Test.Add(new Vector2(0, 2), "wumpus"); Looks for a node at <0,2> and wants to make it a wumpus
///          Test.Add(new Vector2(1, 2), "gold"); Looks for a node at <1,2> and wants to make it a gold
///          Test.Add(new Vector2(3, 2), "goal"); Looks for a node at <3,2> and wants to make it the goal node
///          SetGridValues(Test);
/// If you do not want to set up a static grid you can set the value of isRandom to true in the inspector and the 
/// grid will have randomly set pits, wumpus, gold, and goal positions. 
/// </summary>

public class Grid : MonoBehaviour
{
    public List<Node> Nodes;
    public int Width, Height;
    public int NumPits;
    public int NumWumpus;
    public int NumGold;
    public Node Entrance;
    public Node Exit;
    public bool isRandom;

    private void Awake()
    {
        if (Nodes.Count > 0)
            return;
        GenerateNodes();
        if (isRandom)
        {
            ResetGrid();
            RandomizeNodes();
            TestGrid();
        }
        else
        {
            Dictionary<Vector2, string> DefaultGrid = new Dictionary<Vector2, string>();
            DefaultGrid.Add(new Vector2(0, 0), "start");
            DefaultGrid.Add(new Vector2(2, 0), "pit");
            DefaultGrid.Add(new Vector2(2, 2), "pit");
            DefaultGrid.Add(new Vector2(3, 3), "pit");
            DefaultGrid.Add(new Vector2(0, 2), "wumpus");
            DefaultGrid.Add(new Vector2(1, 2), "gold");
            SetGridValues(DefaultGrid);
        }
    }

    public void SetGridValues(Dictionary<Vector2, string> NodeSet)
    {
        foreach(var index in NodeSet)
        {
            if(GetNode(index.Key) != null)
            {
                switch(index.Value)
                {
                    case "start":
                        this.Entrance = GetNode(index.Key);
                        break;
                    case "gold":
                        GetNode(index.Key).Habitant = Node.Habitants.GOLD;
                        break;
                    case "pit":
                        GetNode(index.Key).Habitant = Node.Habitants.PIT;
                        break;
                    case "wumpus":
                        GetNode(index.Key).Habitant = Node.Habitants.WUMPUS;
                        break;
                    case "goal":
                        this.Exit = GetNode(index.Key);
                        break;
                }
            }
        }
        if(Exit == null)
        {
            foreach(var node in Nodes)
            {
                if (node.Habitant == Node.Habitants.GOLD)
                    Exit = node;
            }
        }

        foreach (var node in this.Nodes)
            node.GetNeighbors(this);
    }

    public Node GetNode(Vector2 position)
    {
        foreach(var node in this.Nodes)
        {
            if (node.Position == position)
                return node;
        }
        return null;
    }

    [ContextMenu("Test")]
    void TestGrid()
    {
        var test = new GridTest(Nodes,Entrance,Exit);
        var isPath = test.AStar();
        if (isPath == false)
        {
            Debug.Log("NoPath");
            RandomizeNodes();
        }
    }

    [ContextMenu("Reset")]
    void ResetGrid()
    {
        this.Entrance = null;
        this.Exit = null;
        foreach(var node in this.Nodes)
        {
            node.Habitant = Node.Habitants.NONE;
            node.name = node.Position.x.ToString() + node.Position.y.ToString();
        }
    }

    [ContextMenu("Randomize")]
    void RandomizeNodes()
    {
        ResetGrid();
        PlaceStart();
        PlacePits();
        PlaceWumpus();
        PlaceGold();       
    }

    void PlaceStart()
    {
        this.Entrance = this.Nodes[Random.Range(0, this.Nodes.Count)];
        if(Entrance.Habitant != Node.Habitants.NONE)
        {
            PlaceStart();
        }
        Entrance.name += " Entrance";
    }

    void PlacePits()
    {
        var currentPitCount = 0;
        foreach(var node in this.Nodes)
        {
            if (node == this.Entrance)
                continue;
            if (node.Habitant != Node.Habitants.NONE)
                continue;
            var index = Random.Range(0, 100);
            if (index % 5 == 0)
            {
                node.Habitant = Node.Habitants.PIT;
                node.name += " Pit";
                currentPitCount++;
            }
        }
    }

    void PlaceWumpus()
    {
        var currentWumpusCount = 0;
        foreach (var node in this.Nodes)
        {
            if (node == this.Entrance)
                continue;
            if (node.Habitant != Node.Habitants.NONE)
                continue;
            var index = Random.Range(0, 100);
            if (index % 5 == 0 && currentWumpusCount < NumWumpus)
            {
                node.Habitant = Node.Habitants.WUMPUS;
                node.name += " Wumpus";
                currentWumpusCount++;
            }
        }
    }

    void PlaceGold()
    {
        var currentGoldCount = 0;
        foreach (var node in this.Nodes)
        {
            if (node == this.Entrance)
                continue;
            if (node.Habitant != Node.Habitants.NONE)
                continue;
            var index = Random.Range(0, 100);
            if (index % 5 == 0 && currentGoldCount < NumGold)
            {
                node.Habitant = Node.Habitants.GOLD;
                Exit = node;
                node.name += " Gold";
                currentGoldCount++;
            }
        }
    }

    [ContextMenu("Gen Grid")]
    void GenerateNodes()
    {
        var xOffSet = 0;
        var yOffSet = 0;
        this.Nodes = new List<Node>();
        for(var i = 0; i < this.Width; i++)
        {
            yOffSet = 0;
            for(var j = 0; j < this.Height; j++)
            {
                var newNode = Instantiate(Resources.Load("NodePrefab",typeof(Node)), this.transform) as Node;
                newNode.Position = new Vector2(i, j);
                newNode.transform.position = newNode.Position + new Vector2(xOffSet, yOffSet);
                newNode.name = i.ToString() + j.ToString();
                this.Nodes.Add(newNode);
                yOffSet += 1;
            }
            xOffSet += 1;
        }
    }
 
    [ContextMenu("Delete")]
    void DeleteGrid()
    {
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if(child != this.transform)
                DestroyImmediate(child.gameObject);
        }
        this.Nodes = new List<Node>();
    }   
}

#region Test
class NodeTest
{
    public Vector2 Position;
    bool Walkable;
    public NodeTest Parent;

    public float gscore = 0;
    public float hscore = 0;
    public float fscore = 0;

    public NodeTest(Node node)
    {
        Position = node.Position;
        if (node.Habitant == Node.Habitants.NONE || node.Habitant == Node.Habitants.GOLD)
        {
            Walkable = true;
        }
        else
            Walkable = false;
    }

    public List<NodeTest> GetNeighbors(List<NodeTest> graph)
    {
        List<NodeTest> ret = new List<NodeTest>();
        List<Vector2> ValidNeighborPositions = new List<Vector2>()
        {
            this.Position + new Vector2(0, 1),
            this.Position + new Vector2(0, -1),
            this.Position + new Vector2(-1, 0),
            this.Position + new Vector2(1, 0)
        };
        foreach (var node in graph)
        {
            foreach (var position in ValidNeighborPositions)
            {
                if (node.Position == position && node.Walkable)
                {
                    ret.Add(node);
                }
            }
        }
        return ret;
    }
}

class GridTest
{
    List<NodeTest> Nodes;
    NodeTest Start;
    NodeTest Goal; 

    public GridTest(List<Node> nodes, Node start, Node goal)
    {
        Nodes = new List<NodeTest>();
        foreach(var node in nodes)
        {
            NodeTest newNode = new NodeTest(node);
            if (node == start)
                Start = newNode;
            if (node == goal)
                Goal = newNode;
            Nodes.Add(newNode);
        }
    }

    void CalcGscore(NodeTest currentNode, NodeTest neighbor)
    {
        var tentativeG = currentNode.gscore + 10;
        if (neighbor.gscore > tentativeG)
        {
            neighbor.gscore = tentativeG;
            neighbor.Parent = currentNode;
        }
        else
        {
            neighbor.gscore = tentativeG;
            neighbor.Parent = currentNode;
        }
    }

    void calcHScore(NodeTest currentNode, NodeTest Goal)
    {
        currentNode.hscore = 10 * (Mathf.Abs(Goal.Position.x - currentNode.Position.x) + Mathf.Abs(Goal.Position.y - currentNode.Position.y));
    }

    void calcFScore(NodeTest node)
    {
        node.fscore = node.gscore + node.hscore;
    }

    public bool AStar()
    {
        List<NodeTest> Path = new List<NodeTest>();
        List<NodeTest> open = new List<NodeTest>();
        List<NodeTest> close = new List<NodeTest>();
        var current = Start;
        open.Add(current);

        if(Start == Goal)
        {
            return true;
        }

        while(current != null)
        {
            open.Remove(current);            
            close.Add(current);
            foreach(var node in current.GetNeighbors(Nodes))
            {
                if(!close.Contains(node))
                {
                    CalcGscore(current, node);
                    calcHScore(node, Goal);
                    calcFScore(node);
                    if (!open.Contains(node))
                        open.Add(node);
                }

            }
            sortOpen(open);
            if (open.Count == 0)
            {
                return false;
            }
            if (current == this.Goal)
                break;
            current = open[0];
        }                

        var retraceCurrent = Goal;
        while(retraceCurrent.Parent != null)
        {
            Path.Add(retraceCurrent);
            retraceCurrent = retraceCurrent.Parent;
        }

        foreach(var node in Path)
        {
            Debug.Log(node.Position);
        }
        return true;
    }

    void sortOpen(List<NodeTest> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            for(int j = 0; j < list.Count; j++)
            {
                if(list[i].fscore < list[j].fscore)
                {
                    NodeTest temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
        }
    }
}
#endregion
