using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTarget : MazeEntity {

    public Game_Manager gameManager;

    public void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<Game_Manager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wand"))
        {
            gameManager.NextLevel();
        }
    }

}
