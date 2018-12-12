using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private MazeCell currentCell;
    private MazeCell targetCell;
    private List<MazeCell> path;

    public void SetLocation(MazeCell cell)
    {
        currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
    }

    private void Move(MazeDirection direction)
    {
        MazeCellEdge edge = currentCell.GetEdge(direction);
        if (edge is MazePassage)
        {
            SetLocation(edge.otherCell);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(MazeDirection.North);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(MazeDirection.East);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(MazeDirection.South);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(MazeDirection.West);
        }
    }

    public void SetTargetCell(MazeCell cell)
    {
        targetCell = cell;
        path = AStar.resolvePath(currentCell, targetCell);
        Debug.Log("player " +path == null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 1);
        if (path != null)
        {
            foreach(MazeCell mc in path)
            {
                //Renderer rend2 = mc.transform.GetChild(0).GetComponent<Renderer>();
                //rend2.material.color = Color.blue;
                Vector3 pos = new Vector3(mc.transform.position.x, -0.8f, mc.transform.position.z);
                Gizmos.DrawCube(pos, Vector3.one*0.02f);
            }
        }
    }


}

