using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WordSpawner))]
public class WordSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WordSpawner spawner = (WordSpawner)target;

        GUILayout.Space(10);

        GUI.enabled = Application.isPlaying;
        if (GUILayout.Button("Spawn eerste woord (test in Play mode)"))
        {
            spawner.SpawnNextWord();
        }
        GUI.enabled = true;

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Deze knop werkt alleen tijdens Play mode.", MessageType.Info);
        }
    }
}