using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IntVector2
{
    [Range(1,50)]
    public int x, z;

    public IntVector2(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static IntVector2 operator + (IntVector2 a, IntVector2 b)
    {
        a.x += b.x;
        a.z += b.z;
        return a;
    }

    public int dist(IntVector2 a)
    {
        return Mathf.Abs(this.x - a.x) + Mathf.Abs(this.z - a.z);
    }
}

