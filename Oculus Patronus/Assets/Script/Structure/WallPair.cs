using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class WallPair : MonoBehaviour
{
    public WallPair(MazeWall a, int b)
    {
        A = a;
        B = b;
    }

    public MazeWall A;
    public int B;
}
