using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    //Oculus Version
    
    //position, size and scale
    public IntVector2 size;
    public Vector3 centerPosition;
    public float scale;

    //prefab
    public MazeCell cellPrefab;
    public MazePassage passagePrefab;
    public MazeWall[] wallPrefabs;
    //public KeyValuePair<MazeWall, int>[] wallPrefabs;
    public MazeDoor doorPrefab;
    public MazeEnemy ennemyPrefab;
    public Transform roof;

    [Range(0, 100)]
    public int numberOfEnemy;

    //probability
    //[Range(0f, 1f)]
    //public float ennemyProbability;
    [Range(0f, 1f)]
    public float doorProbability;

    //settings
    public MazeRoomSettings[] roomSettings;
    public MazeEntity targetTransform;
    public MazeTarget targetPrefab;

    private List<MazeRoom> rooms = new List<MazeRoom>();
    private MazeCell[,] cells;
    private int[,] entityMap;

    public MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    public void Generate()
    {
        cells = new MazeCell[size.x, size.z];
        entityMap = new int[size.x, size.z];

        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            DoNextGenerationStep(activeCells);
        }
        AddEnemy();
        CreateTarget();

        Transform obj = Instantiate(roof);
        obj.parent = transform;
        obj.position = new Vector3(0, 2 * scale, 0);
        obj.localScale = new Vector3(size.x * scale, size.z * scale, 1);

        foreach(MazeRoom room in rooms)
        {
            Debug.Log("room " + room.settingsIndex);
            for(int i = 0; i < room.nbWallInRoom.Length; ++i)
            {
                Debug.Log(room.nbWallInRoom[i] + " " + room.Settings.nbIterationOfWalls[i]);
            }
        }
    }

    public void Generate(LevelSettings levelSettings)
    {
        this.numberOfEnemy = levelSettings.numberOfEnemy;
        this.roomSettings = levelSettings.roomSettings;
        this.size = levelSettings.size;
        this.targetPrefab = levelSettings.target;
        Generate();
    }

    public MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3((coordinates.x - size.x * 0.5f + 0.5f) * scale + centerPosition.x, 0f + centerPosition.y, (coordinates.z - size.z * 0.5f + 0.5f) * scale + centerPosition.z);
        newCell.transform.localScale = newCell.transform.localScale * scale;
        return newCell;
    }

    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        MazeCell newCell = CreateCell(RandomCoordinates);
        newCell.Initialize(CreateRoom(-1));
        activeCells.Add(newCell);
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];
        if(currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (ContainsCoordinates(coordinates))
        {
            MazeCell neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else if (currentCell.room.settingsIndex == neighbor.room.settingsIndex)
            {
                CreatePassageInSameRoom(currentCell, neighbor, direction);
            }
            else
            {
                CreateWall(currentCell, neighbor, direction);
            }
        }
        else
        {
            CreateWall(currentCell, null, direction);
        }
    }

    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
        //if(prefab == passagePrefab)
        //{
        //    prefab = Random.value < ennemyProbability ? ennemyPrefab : passagePrefab;
        //}

        MazePassage passage = Instantiate(prefab) as MazePassage;
        passage.transform.localScale = passage.transform.localScale * scale;

        passage.Initialize(cell, otherCell, direction);

        passage = Instantiate(prefab) as MazePassage;
        passage.transform.localScale = passage.transform.localScale * scale;
        if (passage is MazeDoor)
        {
            otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
        }
        else
        {
            otherCell.Initialize(cell.room);
        }
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreatePassageInSameRoom(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.transform.localScale = passage.transform.localScale * scale;
        passage.Initialize(cell, otherCell, direction);

        passage = Instantiate(passagePrefab) as MazePassage;
        passage.transform.localScale = passage.transform.localScale * scale;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
        if (cell.room != otherCell.room)
        {
            MazeRoom roomToAssimilate = otherCell.room;
            cell.room.Assimilate(roomToAssimilate);
            rooms.Remove(roomToAssimilate);
            Destroy(roomToAssimilate);
        }
    }

    //private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    //{
    //    MazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
    //    wall.transform.localScale = new Vector3(wall.transform.localScale.x * scale, wall.transform.localScale.y * scale, wall.transform.localScale.z * scale);
    //    wall.Initialize(cell, otherCell, direction);
    //    if (otherCell != null)
    //    {
    //        wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
    //        wall.Initialize(otherCell, cell, direction.GetOpposite());
    //        //wall.transform.localScale = wall.transform.localScale * scale;
    //        wall.transform.localScale = new Vector3(wall.transform.localScale.x * scale, wall.transform.localScale.y * scale, wall.transform.localScale.z * scale);
    //    }
    //}

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        int[] nbIterationOfWalls = cell.room.nbWallInRoom;
        int idx;
        do
        {
            idx = Random.Range(0, nbIterationOfWalls.Length);
        } while (nbIterationOfWalls[idx] >= cell.room.Settings.nbIterationOfWalls[idx]);

        MazeWall wall = Instantiate(cell.room.Settings.wallPrefabs[idx]) as MazeWall;
        nbIterationOfWalls[idx]++;

        wall.transform.localScale = new Vector3(wall.transform.localScale.x * scale, wall.transform.localScale.y * scale, wall.transform.localScale.z * scale);
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null)
        {
            nbIterationOfWalls = otherCell.room.nbWallInRoom;
            do
            {
                idx = Random.Range(0, nbIterationOfWalls.Length);
            } while (nbIterationOfWalls[idx] >= otherCell.room.Settings.nbIterationOfWalls[idx]);

            wall = Instantiate(otherCell.room.Settings.wallPrefabs[idx]) as MazeWall;
            nbIterationOfWalls[idx]++;

            //wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
            //wall.transform.localScale = wall.transform.localScale * scale;
            wall.transform.localScale = new Vector3(wall.transform.localScale.x * scale, wall.transform.localScale.y * scale, wall.transform.localScale.z * scale);
        }
    }

    private MazeRoom CreateRoom(int indexToExclude)
    {
        MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
        newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
        if (newRoom.settingsIndex == indexToExclude)
        {
            newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
        }
        newRoom.Settings = roomSettings[newRoom.settingsIndex];
        rooms.Add(newRoom);
        return newRoom;
    }


    void AddEnemy(){
        int enemyCreated = 0;

        int maxEnemy = numberOfEnemy < size.x * size.z - 1 ? numberOfEnemy : size.x * size.z - 1;
        Debug.Log(maxEnemy);
        while (enemyCreated < maxEnemy)
        {
            IntVector2 cellCoordinate = RandomCoordinates;
            if(entityMap[cellCoordinate.x,cellCoordinate.z] ==0)
            {
                MazeEnemy enemy = Instantiate(ennemyPrefab) as MazeEnemy;
                //passage.transform.localScale = passage.transform.localScale * scale;
                enemy.Initialize(cells[cellCoordinate.x, cellCoordinate.z], (MazeDirection)Random.Range(0,3));
                entityMap[cellCoordinate.x, cellCoordinate.z] = 1;
                enemyCreated++;
            }
        }
    }

    private void CreateTarget()
    {
        if (targetTransform != null)
        {
            MazeEntity targetTransformInstance = Instantiate(targetTransform) as MazeEntity;
            IntVector2 cellCoordinate = RandomCoordinates;
            targetTransformInstance.Initialize(cells[cellCoordinate.x, cellCoordinate.z]);
            if (targetPrefab != null)
            {
                MazeTarget targetInstance = Instantiate(targetPrefab) as MazeTarget;
                targetInstance.transform.parent = targetTransformInstance.transform;
                targetInstance.transform.localPosition = new Vector3();
            }
        }
    }
}
