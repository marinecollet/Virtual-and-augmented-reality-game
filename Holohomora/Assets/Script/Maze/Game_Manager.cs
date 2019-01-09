using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows.Speech;
using UnityEngine;

public class Game_Manager : MonoBehaviour {
    public static bool isSetup = false;

    public Maze mazePrefab;
    public Player dobbyInstance;
    public Socket socketPrefab;
    public LevelSettings[] levels;

    private Maze mazeInstance;
    //private Player dobbyInstance;
    private Socket socketInstance;
    private int actualLevel;


    void Start ()
    {
        actualLevel = 1;
        StartCoroutine(BeginGame());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    IEnumerator BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        if (levels != null && levels.Length > 0)
        {
            yield return StartCoroutine(mazeInstance.Generate(levels[actualLevel - 1]));
        }
        else
        {
            yield return StartCoroutine(mazeInstance.Generate());
        }

        dobbyInstance.gameObject.SetActive(true);
        dobbyInstance.SetLocation(mazeInstance.GetCell(new IntVector2(0,0)));
        socketInstance = Instantiate(socketPrefab) as Socket;
        socketInstance.SetLocation(mazeInstance.GetCell(new IntVector2(mazeInstance.size.x-1, mazeInstance.size.z-1)));
        if (levels != null && levels.Length > 0)
        {
            socketInstance.SetMaterial(levels[actualLevel - 1].targetMat);
        }
        isSetup = true;
    }

    private void FixedUpdate()
    {
        if(isSetup)//dobbyInstance && socketInstance)
        {
            if (dobbyInstance.currentCell == socketInstance.currentCell && isSetup)
            {
                isSetup = false;
                StartNextLevel();
            }
        }
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        if(mazeInstance.gameObject != null)
            Destroy(mazeInstance.gameObject);
        dobbyInstance.reset();
        dobbyInstance.gameObject.SetActive(false);

        if (socketInstance != null)
        {
            Destroy(socketInstance.gameObject);
        }
        isSetup = false;
        AStar.Reset();
        actualLevel = 1;
        StartCoroutine(BeginGame());
    }

    void StartNextLevel()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);

        

        if (socketInstance != null)
        {
            Destroy(socketInstance.gameObject);
        }
        
        AStar.Reset();

        if (levels != null && levels.Length > 0)
        {
            actualLevel++;
            if (actualLevel > levels.Length)
            {
                actualLevel--;
                dobbyInstance.win();
            }
            else
            {
                dobbyInstance.gameObject.SetActive(false);
                StartCoroutine(GenerateNextLevel());
            }
        }
        else
        {
            dobbyInstance.gameObject.SetActive(false);
        }
    }

    IEnumerator GenerateNextLevel()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        if (levels != null && levels.Length > 0)
        {
            yield return StartCoroutine(mazeInstance.Generate(levels[actualLevel - 1]));
        }
        else
        {
            yield return StartCoroutine(mazeInstance.Generate());
        }
        Debug.Log("done");
        dobbyInstance.gameObject.SetActive(true);
        dobbyInstance.SetLocation(mazeInstance.GetCell(new IntVector2(0, 0)));
        socketInstance = Instantiate(socketPrefab) as Socket;
        socketInstance.SetLocation(mazeInstance.GetCell(new IntVector2(mazeInstance.size.x - 1, mazeInstance.size.z - 1)));
        socketInstance.SetMaterial(levels[actualLevel - 1].targetMat);
        isSetup = true;
    }
}
