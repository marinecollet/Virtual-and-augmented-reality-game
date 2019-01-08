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

        public MazeCellWeight(MazeCell cell, float weight/*, List<MazeCell> path*/)
        {
            this.cell = cell;
            this.weight = weight;
            //this.path = path;
        }

        public bool Equals(MazeCellWeight other)
        {
            //if (other == null)
            //    return false;
            ////Debug.Log(this.cell.coordinates.x + " " + this.cell.coordinates.z + " " + other.cell.coordinates.x + " " + other.cell.coordinates.z + " " + (this.cell.coordinates == other.cell.coordinates));
            //if (this.cell.coordinates != other.cell.coordinates)
            //    return false;
            //else if(this.weight > other.weight)
            //    return false;
            //else
            //    return true;

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

        //gradient = new Gradient();
        //GradientColorKey[] colorKey;
        //GradientAlphaKey[] alphaKey;

        //// Populate the color keys at the relative time 0 and 1 (0 and 100%)
        //colorKey = new GradientColorKey[2];
        //colorKey[0].color = Color.red;
        //colorKey[0].time = 0.0f;
        //colorKey[1].color = Color.blue;
        //colorKey[1].time = 1.0f;

        //// Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        //alphaKey = new GradientAlphaKey[2];
        //alphaKey[0].alpha = 1.0f;
        //alphaKey[0].time = 0.0f;
        //alphaKey[1].alpha = 0.0f;
        //alphaKey[1].time = 1.0f;

        //gradient.SetKeys(colorKey, alphaKey);
        //gradient.mode = GradientMode.Blend;

        //if (openList != null)
        //{
        //    foreach (MazeCellWeight mc in openList)
        //    {
        //        Renderer rend = mc.cell.transform.GetChild(0).GetComponent<Renderer>();
        //        rend.material = mc.cell.room.settings.floorMaterial;
        //    }
        //}

        //if (closeList != null)
        //{
        //    foreach (MazeCellWeight mc in closeList)
        //    {
        //        Renderer rend = mc.cell.transform.GetChild(0).GetComponent<Renderer>();
        //        rend.material = mc.cell.room.settings.floorMaterial;
        //    }
        //}

        MazeCellWeight start = new MazeCellWeight(current, current.coordinates.distance(target.coordinates));
        openList = new List<MazeCellWeight>();
        closeList = new List<MazeCellWeight>();

        //LinkedList<MazeCell> listCellParsed = new LinkedList<MazeCell>();

        openList.Add(start);
        int i = 0;
        while (openList.Count != 0 && i < 100)
        {
            //Debug.Log(openList.Count);
            MazeCellWeight mazeCellWeight = openList[0];
            //Debug.Log("closeContain "+closeList.Contains(mazeCellWeight));
            //Debug.Log("astar " + mazeCellWeight.cell.coordinates.x+" "+ mazeCellWeight.cell.coordinates.z);

            if (mazeCellWeight.cell.coordinates == target.coordinates)
            {
                openList.Remove(mazeCellWeight);
                closeList.Add(mazeCellWeight);

                //Renderer rend = mazeCellWeight.cell.transform.GetChild(0).GetComponent<Renderer>();
                Debug.Log("closed " + mazeCellWeight.cell.coordinates.x + " " + mazeCellWeight.cell.coordinates.z + " | " + (float)mazeCellWeight.weight);
                //rend.material.color = gradient.Evaluate((mazeCellWeight.weight - min) / (max - min));
                Debug.Log("min " + min + " max " + max);

                //foreach (MazeCellWeight mc in closeList)
                //{
                //    rend = mc.cell.transform.GetChild(0).GetComponent<Renderer>();
                //    //Debug.Log("closed "+mc.cell.coordinates.x+" "+ mc.cell.coordinates.z + " | " + (float)mc.weight);
                //    rend.material.color = gradient.Evaluate((mc.weight - min) / (max - min));
                //}

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

        //Debug.Log("end " + openList.Count +" "+i);
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

        //Debug.Log(path.Count +" | "+ path[path.Count - 1].coordinates.x + " " + path[path.Count - 1].coordinates.z + " | " + mazeCellWeight.cell.coordinates.x + " " + mazeCellWeight.cell.coordinates.z);
        while (!path[path.Count - 1].Equals(start.cell) && i < 20)
        {
            //Debug.Log("i= " + i);
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
                //Debug.Log("fail a star");
                return null;
            }
            else
            {
                path.Add(candidatesWeight.cell);
                currentWeight = candidatesWeight;
                //Debug.Log("current "+currentWeight.cell.coordinates.x + " " + currentWeight.cell.coordinates.z + "  " + (float)currentWeight.weight);
                candidatesWeight = null;
            }
            i++;
            //Debug.Log(path[path.Count - 1].coordinates.x + " " + path[path.Count - 1].coordinates.z + "  " + start.cell.coordinates.x + " " + start.cell.coordinates.z);
        }

        //Debug.Log("path count " + path.Count);
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
