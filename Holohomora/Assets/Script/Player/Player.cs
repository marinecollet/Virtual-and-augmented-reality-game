using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public MazeCell currentCell { get; private set; }
    public int lifeAtStart;
    public int lifeLosePerShot;
    public bool isDead { get; private set; }
    private Teleport teleport;

    private MazeCell targetCell;
    private MazeCell movingCell;
    private List<MazeCell> path;
    private int life;


    private bool haveWin = false;
    private bool isMoving = false;
    private bool isHurt = false;
    private Animator anim;

    public void Awake()
    {
        anim = GetComponent<Animator>();
        isDead = false;
        life = lifeAtStart;
    }

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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyShot"))
        {
            Destroy(other.gameObject);
            this.life -= lifeLosePerShot;
            if(isMoving)
                anim.SetBool("isWalking", false);
            isHurt = true;
            anim.SetBool("isHurt", true);
            if (life <= 0)
            {
                anim.SetBool("isDead", true);
                isDead = true;
            }
        }
    }

    private void Update()
    {
        if (isMoving && !isHurt && !isDead && !haveWin)
        {
            Vector3 dir = movingCell.transform.position - this.transform.position;
            dir.Normalize();
            this.transform.position += dir * speed * Time.deltaTime;
            Quaternion q = Quaternion.LookRotation(dir);
            this.transform.localRotation = q;
            if ((movingCell.transform.position - this.transform.position).sqrMagnitude < 0.001)
            {
                currentCell = movingCell;
                if (movingCell == targetCell)
                {
                    isMoving = false;
                    anim.SetBool("isWalking", false);
                }
                else
                {
                    //bug when moving and going where we can't
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
            anim.SetBool("isWalking", true);
        }
        else
        {
            teleport = GameObject.Find("Launch Mesh(Clone)").GetComponent<Teleport>();
            if(teleport != null)
                teleport.unValidTp();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 1);
        if (path != null)
        {
            foreach(MazeCell mc in path)
            {
                if(mc != null)
                {
                    Vector3 pos = new Vector3(mc.transform.position.x, -0.8f, mc.transform.position.z);
                    Gizmos.DrawCube(pos, Vector3.one * 0.02f);
                }
            }
        }
    }

    public void HurtFinished()
    {
        if (!isDead)
        {
            if (isMoving)
                anim.SetBool("isWalking", true);

            isHurt = false;
            anim.SetBool("isHurt", false);
        }
    }

    public void reset()
    {
        life = lifeAtStart;
        haveWin = false;
        isMoving = false;
        isHurt = false;
        isDead = false;
    }

    public void win()
    {
        haveWin = true;
        anim.SetBool("haveWin", true);
    }
}

