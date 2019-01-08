using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu()]
public class MazeRoomSettings : ScriptableObject{

    public Material floorMaterial, wallMaterial;
    public MazeWall[] wallPrefabs;
    [SerializeField]
    public int[] nbIterationOfWalls;
}
