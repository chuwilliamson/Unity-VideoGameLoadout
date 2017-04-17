using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public List<Node> Nodes;
    public int Width, Height;
    public int NumPits;
    public int NumWumpus;
    public int NumGold;
    public Node Entrance;
    public Node Exit;

    private void Awake()
    {
        
    }

    [ContextMenu("Test")]
    void TestGrid()
    {
        var test = new GridTest(Nodes,Entrance,Exit);
        var isPath = test.AStar();
        //if (!isPath)
        //{
        //    Debug.Log("NoPath");
        //    RandomizeNodes();
        //}
    }

    [ContextMenu("Reset")]
    void ResetGrid()
    {
        foreach(var node in this.Nodes)
        {
            node.Habitant = Node.Habitants.NONE;
            node.name = "Node";
        }
    }

    [ContextMenu("Randomize")]
    void RandomizeNodes()
    {
        ResetGrid();
        PlacePits();
        PlaceWumpus();
        PlaceGold();
        PlaceStart();        
    }

    void PlaceStart()
    {
        Entrance = this.Nodes[Random.Range(0, this.Nodes.Count)];
        Entrance.name = "Entrance";
        Entrance.changeColor(Color.green);
        if(Entrance.Habitant != Node.Habitants.NONE)
        {
            PlaceStart();
        }
    }

    void PlacePits()
    {
        var currentPitCount = 0;
        foreach(var node in this.Nodes)
        {
            if (node.Habitant != Node.Habitants.NONE)
                continue;
            var index = Random.Range(0, 100);
            if (index % 5 == 0)
            {
                node.Habitant = Node.Habitants.PIT;
                node.name = "Pit " + currentPitCount;
                currentPitCount++;
            }
        }
    }

    void PlaceWumpus()
    {
        var currentWumpusCount = 0;
        foreach (var node in this.Nodes)
        {
            if (node.Habitant != Node.Habitants.NONE)
                continue;
            var index = Random.Range(0, 100);
            if (index % 5 == 0 && currentWumpusCount < NumWumpus)
            {
                node.Habitant = Node.Habitants.WUMPUS;
                node.name = "Wumpus " + currentWumpusCount;
                currentWumpusCount++;
            }
        }
    }

    void PlaceGold()
    {
        var currentGoldCount = 0;
        foreach (var node in this.Nodes)
        {
            if (node.Habitant != Node.Habitants.NONE)
                continue;
            var index = Random.Range(0, 100);
            if (index % 5 == 0 && currentGoldCount < NumGold)
            {
                node.Habitant = Node.Habitants.GOLD;
                Exit = node;
                node.name = "Gold " + currentGoldCount;
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
                break;
            }
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

        if(open.Count == 0)
            return false;
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
