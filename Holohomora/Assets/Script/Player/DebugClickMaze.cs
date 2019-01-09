using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugClickMaze : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}

    void OnMouseDown()
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        MazeCell parent = this.transform.GetComponentInParent<MazeCell>();
        player.SetTargetCell(parent);
    }
}
