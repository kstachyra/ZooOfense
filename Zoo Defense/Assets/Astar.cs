using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    public int columns;
    public int rows;

    public int startX;
    public int startY;

    Node end = new Node();
    Node start = new Node();

    bool[,] Grid;

    HashSet<Node> ClosedSet = new HashSet<Node>();
    HashSet<Node> OpenSet = new HashSet<Node>();

    //poprzednik
    Node[,] CameFrom;

    //start -> node score
    float[,] gScore;

    //start -> node -> goal score
    float[,] fScore;

    class Node
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
            if (other == null)
            {
                return false;
            }

            var obj = other as Node;

            if (this.X == obj.X && this.Y == obj.Y)
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


    public void Init (int height, int width)
    {
        columns = width;
        rows = height;

        start = new Node(startX, startY);

        Grid = new bool[columns, rows];

        fScore = new float[columns, rows];
        gScore = new float[columns, rows];
        CameFrom = new Node[columns, rows];

        end.X = 0;
        end.Y = rows / 2;

        OpenSet.Add(new Node(start.X, start.Y));

        for (int i=0; i<columns; ++i)
        {
            for (int j=0; j<rows; ++j)
            {
                gScore[i, j] = float.MaxValue;
                fScore[i, j] = float.MaxValue;

                if (Grid[i, j] == true) ClosedSet.Add(new Node(i, j));
            }
        }

        //start -> start = 0
        gScore[startX, startY] = 0;

        //start - >start-> end score
        fScore[startX, startY] = h(start, end);

        go();
	}

    private void go()
    {
        Node best = new Node();
        while (OpenSet.Count > 0)
        {
            bool first = true;
            foreach (Node node in OpenSet)
            {
                if (first)
                {
                    best = node;
                    first = false;
                }
                if (fScore[node.X, node.Y] < fScore[best.X, best.Y])
                {
                    best = node;
                }
            }


            if (best.Equals(end))
            {
                printPath(start, end);
                return;
            }

            OpenSet.Remove(best);
            ClosedSet.Add(best);

            HashSet<Node> neighbours = new HashSet<Node>();

            neighbours.Add(new Node(best.X - 1, best.Y));
            neighbours.Add(new Node(best.X + 1, best.Y));
            neighbours.Add(new Node(best.X, best.Y -1));
            neighbours.Add(new Node(best.X, best.Y +1));




            foreach (Node node in neighbours)
            {
                if (node.X >= 0 && node.X < columns && node.Y >= 0 && node.Y < rows)
                {
                    if (!ClosedSet.Contains(node))
                    {

                        if (!OpenSet.Contains(node))
                        {
                            OpenSet.Add(node);
                        }

                        float newPath = gScore[best.X, best.Y] + 1;

                        //lepsza ścieżka
                        if (newPath < gScore[node.X, node.Y])
                        {
                            CameFrom[node.X, node.Y] = best;

                            gScore[node.X, node.Y] = newPath;
                            fScore[node.X, node.Y] = gScore[node.X, node.Y] + h(node, end);
                        }
                    }

                }
            }         

        }


    }

    private void printPath(Node start, Node end)
    {
        Node curr = end;

        while (!curr.Equals(start))
        {
            print(curr.X + "." + curr.Y);
            curr = CameFrom[curr.X, curr.Y];
        }
        print(start.X + "." + start.Y);
    }

    //heurtstyczna odległość
    private float h(Node from, Node to)
    {
        return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
    }
}
