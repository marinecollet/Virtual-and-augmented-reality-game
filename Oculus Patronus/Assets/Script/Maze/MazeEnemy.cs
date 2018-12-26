using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEnemy : MonoBehaviour{
    public MazeCell cell;

    public MazeDirection direction;

    public virtual void Initialize(MazeCell cell, MazeDirection direction)
    {
        this.cell = cell;        
        this.direction = direction;
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();
    }
}
