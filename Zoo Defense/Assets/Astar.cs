using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Node
{
    public Node()
    {
        this.X = -1;
        this.Y = -1;
    }

    public Node(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override bool Equals(object other)
    {
        if(other == null)
        {
            return false;
        }

        var obj = other as Node;

        if(this.X == obj.X && this.Y == obj.Y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return this.X * 1000 + this.Y;
    }

    public int X { get; set; }
    public int Y { get; set; }
};

public static class Astar
{
    static int columns;
    static int rows;

    static Node end = new Node();
    static Node start = new Node();

    static bool[,] Grid;

    static HashSet<Node> ClosedSet = new HashSet<Node>();
    static HashSet<Node> OpenSet = new HashSet<Node>();

    //poprzednik
    static Node[,] CameFrom;

    //start -> node score
    static float[,] gScore;

    //start -> node -> goal score
    static float[,] fScore;


    public static void Init(int width, int height)
    {
        columns = width;
        rows = height;

        Grid = new bool[columns, rows];

        fScore = new float[columns, rows];
        gScore = new float[columns, rows];
        CameFrom = new Node[columns, rows];
    }

    public static void SetGrid(bool[,] TowerGrid)
    {
        Grid = TowerGrid;
    }

    public static List<Node> CalcPath(Node s, Node e)
    {
        Clear();

        start = new Node(s.X, s.Y);
        end = new Node(e.X, e.Y);

        OpenSet.Add(new Node(start.X, start.Y));

        for(int i = 0; i < columns; ++i)
        {
            for(int j = 0; j < rows; ++j)
            {
                gScore[i, j] = float.MaxValue;
                fScore[i, j] = float.MaxValue;

                if(Grid[i, j] == true) ClosedSet.Add(new Node(i, j));
            }
        }

        //start -> start = 0
        gScore[start.X, start.Y] = 0;

        //start - >start-> end score
        fScore[start.X, start.Y] = H(start, end);






        Node best = new Node();
        while(OpenSet.Count > 0)
        {
            bool first = true;
            foreach(Node node in OpenSet)
            {
                if(first)
                {
                    best = node;
                    first = false;
                }

                if(fScore[node.X, node.Y] < fScore[best.X, best.Y])
                {
                    best = node;
                }
                else if (fScore[node.X, node.Y] == fScore[best.X, best.Y] && ((UnityEngine.Random.Range(0.0f, 1.0f) > 0.5)))
                {
                        best = node;
                }
            }


            if(best.Equals(end))
            {
                return GetPath(start, end);
            }

            OpenSet.Remove(best);
            ClosedSet.Add(best);

            List<Node> neighbours = new List<Node>
            {
                new Node(best.X - 1, best.Y),
                new Node(best.X + 1, best.Y),
                new Node(best.X, best.Y - 1),
                new Node(best.X, best.Y + 1)
            };

            var rnd = new System.Random();
            var result = neighbours.OrderBy(item => rnd.Next());

            foreach (Node node in neighbours)
            {
                if(node.X >= 0 && node.X < columns && node.Y >= 0 && node.Y < rows)
                {
                    if(!ClosedSet.Contains(node))
                    {

                        if(!OpenSet.Contains(node))
                        {
                            OpenSet.Add(node);
                        }

                        float newPath = gScore[best.X, best.Y] + 1;

                        //lepsza ścieżka
                        if(newPath < gScore[node.X, node.Y])
                        {
                            CameFrom[node.X, node.Y] = best;

                            gScore[node.X, node.Y] = newPath;
                            fScore[node.X, node.Y] = gScore[node.X, node.Y] + H(node, end);
                        }
                    }

                }
            }

        }

        //nie ma ścieżki
        return new List<Node>();
    }


    private static void Clear()
    {
        OpenSet.Clear();
        ClosedSet.Clear();

        for(int i = 0; i < columns; ++i)
        {
            for(int j = 0; j < rows; ++j)
            {
                CameFrom[i, j] = null;
            }
        }
    }

    //@return lista wierzchołków ścieżki od końca do początku
    private static List<Node> GetPath(Node s, Node e)
    {
        List<Node> path = new List<Node>();

        Node curr = e;

        while(curr != null)
        {
            path.Add(curr);
            curr = CameFrom[curr.X, curr.Y];
        }

        return path;
    }

    //heurtstyczna odległość
    private static float H(Node from, Node to)
    {
        return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
    }
}