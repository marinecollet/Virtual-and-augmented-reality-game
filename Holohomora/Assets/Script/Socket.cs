using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour {
    
    public MazeCell currentCell { get; private set; }
    // Use this for initialization
    public void SetLocation(MazeCell cell)
    {
        currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
    }

    public void SetMaterial(Material mat)
    {
        Renderer rend = transform.GetChild(0).GetComponent<Renderer>();
        rend.material = mat;
    }
}
