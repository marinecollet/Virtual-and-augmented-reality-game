using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar{
    private static Gradient gradient;
    private static float max, min;

    class MazeCellWeight : IEquatable<MazeCellWeight>
    {
        public  MazeCell cell;
        public float weight;
        public List<MazeCell> path;

        public MazeCellWeight(MazeCell cell, float weight)
        {
            this.cell = cell;
            this.weight = weight;
        }

        public bool Equals(MazeCellWeight other)
        { 

            if (other == null)
                return false;
           return this.cell.coordinates == other.cell.coordinates;
        }

        public static bool operator < (MazeCellWeight current, MazeCellWeight other)
        {
            return current.weight < other.weight;
        }

        public static bool operator > (MazeCellWeight current, MazeCellWeight other)
        {
            return current.weight > other.weight;
        }
    }

    private static List<MazeCellWeight> openList;
    private static List<MazeCellWeight> closeList;

    public static List<MazeCell> resolvePath(MazeCell current, MazeCell target)
    {
        max = float.MinValue;
        min = float.MaxValue;

        MazeCellWeight start = new MazeCellWeight(current, current.coordinates.distance(target.coordinates));
        openList = new List<MazeCellWeight>();
        closeList = new List<MazeCellWeight>();


        openList.Add(start);
        int i = 0;
        while (openList.Count != 0 && i < 100)
        {
            MazeCellWeight mazeCellWeight = openList[0];

            if (mazeCellWeight.cell.coordinates == target.coordinates)
            {
                openList.Remove(mazeCellWeight);
                closeList.Add(mazeCellWeight);

                return  createPath(start, mazeCellWeight);
            }

            foreach (MazeCellEdge otherCell in mazeCellWeight.cell.GetEdgeList())
            {
                if (otherCell.otherCell != null && otherCell is MazePassage)
                {
                    if(otherCell is MazeDoor)
                    {
                        if(((MazeDoor) otherCell).isOpen)
                        {
                            MazeCellWeight newCellWeight = new MazeCellWeight(otherCell.otherCell, otherCell.otherCell.coordinates.distance(target.coordinates));

                            if (!closeList.Contains(newCellWeight) && !openList.Contains(newCellWeight))
                            {
                                openList.Add(newCellWeight);
                            }
                        }
                    }
                    else
                    {
                        MazeCellWeight newCellWeight = new MazeCellWeight(otherCell.otherCell, otherCell.otherCell.coordinates.distance(target.coordinates));

                        if (!closeList.Contains(newCellWeight) && !openList.Contains(newCellWeight))
                        {
                            openList.Add(newCellWeight);
                        }
                    }
                }
            }
            i++;
            openList.Remove(mazeCellWeight);
            closeList.Add(mazeCellWeight);
            if (mazeCellWeight.weight > max)
            {
                max = mazeCellWeight.weight;
            }
            if (mazeCellWeight.weight < min)
            {
                min = mazeCellWeight.weight;
            }
            openList.Sort(compareMazeCell);
        }

        return null;
    }

    private static List<MazeCell> createPath(MazeCellWeight start, MazeCellWeight mazeCellWeight)
    {
        List<MazeCell> path = new List<MazeCell>();
        path.Add(mazeCellWeight.cell);

        MazeCellWeight candidatesWeight = null;
        MazeCellWeight otherCellWeight;
        MazeCellWeight currentWeight = mazeCellWeight;

        int i = 0;
        while (!path[path.Count - 1].Equals(start.cell) && i < 20)
        {
            foreach (MazeCellEdge otherCell in currentWeight.cell.GetEdgeList())
            {
                if (otherCell.otherCell != null && otherCell is MazePassage && !path.Contains(otherCell.otherCell))
                {
                    otherCellWeight = new MazeCellWeight(otherCell.otherCell, 0);

                    int idx = closeList.IndexOf(otherCellWeight);

                    if (idx != -1)
                    {
                        if (candidatesWeight == null)
                        {
                            candidatesWeight = closeList[idx];
                        }
                        else if (closeList[idx].weight > candidatesWeight.weight)
                        {
                            candidatesWeight = closeList[idx];
                        }
                    }
                }
            }
            if(candidatesWeight == null)
            {
                return null;
            }
            else
            {
                path.Add(candidatesWeight.cell);
                currentWeight = candidatesWeight;
                candidatesWeight = null;
            }
            i++;
        }

        path.Reverse();
        return path;
    }

    private static int compareMazeCell(MazeCellWeight cell1, MazeCellWeight cell2)
    {
        if (cell1.weight < cell2.weight)
            return -1;
        else if (cell1.weight == cell2.weight)
            return 0;
        else
            return 1;
    }

    public static void Reset()
    {
        if (openList != null)
            openList.Clear();

        if (closeList != null)
            closeList.Clear();
    }
}
