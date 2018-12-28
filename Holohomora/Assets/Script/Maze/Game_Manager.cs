using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows.Speech;
using UnityEngine;

public class Game_Manager : MonoBehaviour {
    public static bool isSetup = false;

    public Maze mazePrefab;
    public Player dobbyPrefab;
    public Socket socketPrefab;
    //public float scale;

    private Maze mazeInstance;
    private Player dobbyInstance;
    private Socket socketInstance;


    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void Start ()
    {
        keywords.Add("Next", () =>
        {
            // Call the OnReset method on every descendant object.
            RestartGame();
        });
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        StartCoroutine(BeginGame());
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    IEnumerator BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());
        float tempX = Random.Range(0, mazeInstance.size.x);
        float tempZ = Random.Range(0, mazeInstance.size.z);
        //Vector3 dobbySpawn = new Vector3((tempX - mazeInstance.size.x * 0.5f + 0.5f) * 0.1f + 0, -1, (tempZ - mazeInstance.size.z * 0.5f + 0.5f) * 0.1f + 2);
        dobbyInstance = Instantiate(dobbyPrefab) as Player;
        dobbyInstance.SetLocation(mazeInstance.GetCell(new IntVector2(0,0)));
        socketInstance = Instantiate(socketPrefab) as Socket;
        socketInstance.SetLocation(mazeInstance.GetCell(new IntVector2(mazeInstance.size.x-1, mazeInstance.size.z-1)));
        isSetup = true;
        //dobbyInstance = Instantiate(dobby) as GameObject;
        //dobbyInstance.transform.position = dobbySpawn;
    }

    private void FixedUpdate()
    {
        if(dobbyInstance && socketInstance)
        {
            if (dobbyInstance.currentCell == socketInstance.currentCell && isSetup)
            {
                isSetup = false;
                StartNextLevel();
            }
        }
    }

    void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        if (dobbyInstance != null)
        {
            Destroy(dobbyInstance.gameObject);
        }
        if (socketInstance != null)
        {
            Destroy(socketInstance.gameObject);
        }
        isSetup = false;
        AStar.Reset();
        StartCoroutine(BeginGame());
    }
    void StartNextLevel()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);

        dobbyInstance.gameObject.SetActive(false);

        if (socketInstance != null)
        {
            Destroy(socketInstance.gameObject);
        }
        
        AStar.Reset();

        StartCoroutine(GenerateNextLevel());
    }

    IEnumerator GenerateNextLevel()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());
        Debug.Log("done");
        dobbyInstance.gameObject.SetActive(true);
        dobbyInstance.SetLocation(mazeInstance.GetCell(new IntVector2(0, 0)));
        socketInstance = Instantiate(socketPrefab) as Socket;
        socketInstance.SetLocation(mazeInstance.GetCell(new IntVector2(mazeInstance.size.x - 1, mazeInstance.size.z - 1)));
        isSetup = true;
    }
}
