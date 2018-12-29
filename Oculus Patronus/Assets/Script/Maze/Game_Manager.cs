using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour {
    public static bool isSetup = false;
    public Maze mazePrefab;
    public First_room roomPrefab;
    public LevelSettings[] levels;
    public Player player;

    private Maze mazeInstance;
    private First_room roomInstance;
    private int actualLevel;

    private void Awake()
    {
        actualLevel = 1;
    }

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
        if (levels != null && levels.Length > 0)
        {
            mazeInstance.Generate(levels[actualLevel -1]);
        }
        else
        {
            mazeInstance.Generate();
        }
        isSetup = true;

    }

    void RestartGame()
    {
        //Destroy(roomInstance.gameObject);
        Destroy(mazeInstance.gameObject);
        isSetup = false;

        BeginGame();
    }

    public void NextLevel()
    {
        Destroy(mazeInstance.gameObject);
        if(levels != null && levels.Length > 0) {
            actualLevel++;
            if (actualLevel > levels.Length)
                actualLevel = levels.Length;
        }

        BeginGame();
        player.move(new Vector3(0, 0));
    }
}