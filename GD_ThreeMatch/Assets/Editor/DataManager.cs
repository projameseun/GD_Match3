using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;

public class DataManager : Editor
{
    [MenuItem("Shortcut/Scene/InGame")]
    static void GoInGame()
    {
        EditorSceneManager.OpenScene("Assets/1.Scenes/InGame.unity");
    }


    [MenuItem("Shortcut/Scene/SonMap")]
    static void GoPuzzleMaker()
    {
        EditorSceneManager.OpenScene("Assets/1.Scenes/SonMap.unity");
    }

}
