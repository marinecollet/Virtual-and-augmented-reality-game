using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSettings : ScriptableObject {
    [Range(0,100)]
    public int numberOfEnemy = 0;
    public MazeTarget target= null;
    public MazeRoomSettings[] roomSettings = null;
    public IntVector2 size;
}
