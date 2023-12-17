using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.Requests;

public class PromptRunnerEditor : EditorWindow
{
    string inputText = "";

    [MenuItem("Window/Prompt Runner")]
    public static void ShowWindow()
    {
        GetWindow<PromptRunnerEditor>("Prompt Runner");
    }

    private string textFieldValue;

    void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();
        
        // Calculate the position and size for the text field
        Rect sceneViewRect = SceneView.currentDrawingSceneView.position;
        float textFieldWidth = sceneViewRect.width / 3;
        float textFieldHeight = 30;
        float xPosition = (sceneViewRect.width - textFieldWidth) / 2;
        float yPosition = sceneViewRect.height - textFieldHeight - 40; // 10 pixels from the bottom
        
        GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField) { fontSize = 18 };
        textFieldStyle.alignment = TextAnchor.MiddleCenter;
        textFieldValue = GUI.TextField(new Rect(xPosition, yPosition, textFieldWidth, textFieldHeight), textFieldValue, textFieldStyle);
        
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 18 };
        textFieldStyle.alignment = TextAnchor.MiddleCenter;
        
        if (GUI.Button(new Rect(xPosition + textFieldWidth + 20, yPosition, 70, textFieldHeight), "Do It!", buttonStyle))
        {
            API.SendRequest(CreateRequest(textFieldValue), (response) => HandleResponse(response));
        }
        
        Handles.EndGUI();
    }

    Request CreateRequest(string inputPrompt)
    {
        Request request = new Request();
        request.model = "gpt-3.5-turbo";
        request.messages = new Message[]
        {
            new Message()
            {
                content  = "Write a Unity Editor Script.\n"+
                           "- Provide its functionality as a menu item placed \"Edit\" > \"Do Task\"\n"+
                           "- It doesn't provide any editor window. Immediately execute the task when menu item is invoked.\n"+
                           "- Only script body is required, no explanation needed.",
                role = "system"
            },
            new Message()
            {
                content = inputPrompt,
                role = "user"
            }
        };

        return request;
    }
    
    void OnGUI()
    {
        GUILayout.Label("Enter your text below", EditorStyles.boldLabel);
        inputText = EditorGUILayout.TextField("Prompt", inputText);

        if (GUILayout.Button("Generate"))
        {
            API.SendRequest(CreateRequest(inputText), (response) => HandleResponse(response));
        }
    }

    private void HandleResponse(string response)
    {
        var flags = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic;
        var method = typeof(ProjectWindowUtil).GetMethod("CreateScriptAssetWithContent", flags);
        method.Invoke(null, new object[]{"Assets/Temp.cs", response});
    }





    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
    }

    void OnAfterAssemblyReload()
    {
        if (!System.IO.File.Exists("Assets/Temp.cs")) return;
        EditorApplication.ExecuteMenuItem("Edit/Do Task");
        AssetDatabase.DeleteAsset("Assets/Temp.cs");
    }
    
    
    
    
    
    
}













