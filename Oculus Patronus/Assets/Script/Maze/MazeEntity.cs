using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEntity : MonoBehaviour{
    public MazeCell cell;

    public virtual void Initialize(MazeCell cell)
    {
        this.cell = cell;        
        transform.parent = cell.transform;
    }
}
