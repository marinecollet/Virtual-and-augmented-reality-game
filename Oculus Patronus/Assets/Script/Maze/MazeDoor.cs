using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MazePassage
{

    public override void Initialize(MazeCell primary, MazeCell other, MazeDirection direction)
    {
        base.Initialize(primary, other, direction);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
        }
        MeshRenderer a = this.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        a.material = cell.room.settings.wallMaterial;
    }
}
