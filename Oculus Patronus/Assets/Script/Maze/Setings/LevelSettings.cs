using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSettings : ScriptableObject {
    public MazeEnemy[] enemyType;

    [Range(0, 100)]
    public int[] numberOfEnemy;

    public MazeTarget target= null;
    public MazeRoomSettings[] roomSettings = null;
    public IntVector2 size;
}
