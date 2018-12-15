using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public MazeCell currentCell { get; private set; }
    private MazeCell targetCell;
    private MazeCell movingCell;
    private List<MazeCell> path;

    private bool isMoving = false;

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

        if (isMoving)
        {
            Vector3 dir = movingCell.transform.position - this.transform.position;
            dir.Normalize();
            this.transform.position += dir * speed * Time.deltaTime;
            Quaternion q = Quaternion.LookRotation(-dir);
            this.transform.localRotation = q;
            if ((movingCell.transform.position - this.transform.position).sqrMagnitude < 0.001)
            {
                currentCell = movingCell;
                if (movingCell == targetCell)
                {
                    isMoving = false;
                }
                else
                {
                    int idx = path.IndexOf(movingCell);
                    movingCell = path[idx + 1];
                }
                
            }
        }
    }

    public void SetTargetCell(MazeCell cell)
    {
        targetCell = cell;
        path = AStar.resolvePath(currentCell, targetCell);
        if(path != null)
        {
            movingCell = path[1];
            isMoving = true;
        }
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

