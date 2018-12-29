using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEnemy : MazeEntity {
    public MazeDirection direction;

    public virtual void Initialize(MazeCell cell, MazeDirection direction)
    {
        base.Initialize(cell);
        this.direction = direction;
        transform.localRotation = direction.ToRotation();
        transform.localPosition = Vector3.zero;
    }
}
