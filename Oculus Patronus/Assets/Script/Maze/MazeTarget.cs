using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTarget : MazeEntity {

    public Game_Manager gameManager;

    public void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<Game_Manager>();
        Debug.Log(gameManager);
    }

    public override void Initialize(MazeCell cell)
    {
        this.cell = cell;
        transform.parent = cell.transform;
        transform.localPosition = new Vector3(0,transform.localPosition.y,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wand"))
        {
            gameManager.NextLevel();
        }
    }

}
