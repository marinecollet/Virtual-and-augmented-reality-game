using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour {
    public static bool isSetup = false;
    public Maze mazePrefab;
    public First_room roomPrefab;
    public bool loadMaze;

    private Maze mazeInstance;
    private First_room roomInstance;
 

	void Start ()
    {
        BeginGame();
	}
	
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
	}

    void BeginGame()
    {
        if(loadMaze)
        {
            mazeInstance = Instantiate(mazePrefab) as Maze;
            mazeInstance.Generate();
            isSetup = true;
        }
        else
            roomInstance = Instantiate(roomPrefab) as First_room;
    }

    void RestartGame()
    {
        if (loadMaze)
        {
            Destroy(mazeInstance.gameObject);
            isSetup = false;
        }
        else
            Destroy(roomInstance.gameObject);

        BeginGame();
    }
}
