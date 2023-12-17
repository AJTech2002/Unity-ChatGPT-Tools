using UnityEditor;
using UnityEngine;
 
public class ScenePrompter : Editor
{
    public void OnSceneGUI()
    {
        Handles.BeginGUI();
 
        if (GUILayout.Button("Press Me"))
            Debug.Log("Got it to work.");
 
        Handles.EndGUI();
    }
}