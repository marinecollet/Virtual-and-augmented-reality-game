using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEntity : MonoBehaviour{
    public MazeCell cell;

    public virtual void Initialize(MazeCell cell)
    {
        this.cell = cell;
        Vector3 tempLocalPosition = this.transform.localPosition;
        transform.parent = cell.transform;
        transform.localPosition = tempLocalPosition;
    }
}
