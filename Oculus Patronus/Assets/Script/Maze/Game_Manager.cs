using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour {
    public static bool isSetup = false;
    public Maze mazePrefab;
    public First_room roomPrefab;

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

        //roomInstance = Instantiate(roomPrefab) as First_room;
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.Generate();
        isSetup = true;

    }

    void RestartGame()
    {
        //Destroy(roomInstance.gameObject);
        Destroy(mazeInstance.gameObject);
        isSetup = false;

        BeginGame();
    }
}