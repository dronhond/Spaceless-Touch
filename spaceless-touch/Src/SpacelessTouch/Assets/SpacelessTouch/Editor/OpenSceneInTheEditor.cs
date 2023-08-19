using UnityEditor;
using UnityEditor.SceneManagement;

namespace DarkSword.Editor
{
    public static class OpenSceneInTheEditor
    {
        private static void OpenScene(string str)
        {
            UnityEngine.Debug.Log("[ OpenScene ] = " + str);
            EditorSceneManager.OpenScene(str);
        }

        //      % (ctrl on Windows, cmd on macOS), 
        //      # (shift)
        //      & (alt).
        //     [MenuItem("GameObject/ToggleActivation #&a")] // shift a 
        //     static void ToggleActivation()
        //     {
        //         //Debug.Log("shift AA");
        //         var go = Selection.activeGameObject;
        //         if (go != null)
        //         {
        //             go.SetActive(!go.activeSelf);
        //         }        
        //     }
    
        private static readonly string[] SceneName = {
            "Assets/SpacelessTouch/Scenes/StartScene.unity",
        };

        private static void OpenScene(int index)
        {
            OpenScene(SceneName[index]);
        }

        [MenuItem("Tools/OpenScene/MainGame &q")]
        private static void OpenScene_q()
        {
            OpenScene(0);
        }
    }
}