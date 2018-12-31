using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WallPair))]
public class PairEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WallPair myTarget = (WallPair)target;


        myTarget.B = EditorGUILayout.IntField("nb", myTarget.B);
    }
}