#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System;
using System.IO;

public enum ObjectType
{
    sceneType,
    sessionType,
    transitionType
}

public class CreatePresentationElements
{
    private const string folderPath = "Assets/Data/PresentationObject";
    private const string sessionPath = "Assets/Data/Sessions";
    private const string transitionPath = "Assets/Data/Transitions";
    private const string scenePath = "Assets/Data/Scenes";
    private const string timelinePath = "Assets/Data/Timeline";

    [MenuItem("PresentationData/Create a presentation")]
    public static void CreatePresentation()
    {
        // Check the folder is existed or not. Existed -> save the file to the folder. Not existed -> create a folder and save the file.
        CheckFolderExists(folderPath);

        // Create a presentation scriptable object
        PresentationData newPresentation = ScriptableObject.CreateInstance<PresentationData>();
        AssetDatabase.CreateAsset(newPresentation, AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folderPath, "Presentation.asset")));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("PresentationData/Create sessions, transitions and scenes")]
    public static void CreateOtherObjects()
    {
        CheckFolderExists(scenePath);
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        string newSceneName = NextObjectName(ObjectType.sceneType);
        EditorSceneManager.SaveScene(newScene, AssetDatabase.GenerateUniqueAssetPath(Path.Combine(scenePath, newSceneName + ".unity")));
        AssetDatabase.SaveAssets();

        CheckFolderExists(sessionPath);
        SessionData newSession = ScriptableObject.CreateInstance<SessionData>();
        string newSessionName = NextObjectName(ObjectType.sessionType);
        AssetDatabase.CreateAsset(newSession, AssetDatabase.GenerateUniqueAssetPath(Path.Combine(sessionPath, newSessionName + ".asset")));
        newSession.initialize(newSceneName);
        EditorUtility.SetDirty(newSession);
        AssetDatabase.SaveAssets();

        CheckFolderExists(transitionPath);
        TransitionData newTransition = ScriptableObject.CreateInstance<TransitionData>();
        string newTransitionName = NextObjectName(ObjectType.transitionType);
        AssetDatabase.CreateAsset(newTransition, AssetDatabase.GenerateUniqueAssetPath(Path.Combine(transitionPath, newTransitionName + ".asset")));
        newTransition.initialize(newSession);
        EditorUtility.SetDirty(newTransition);
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();        
    }

    [MenuItem("PresentationData/Create timeline")]
    public static void CreateTimeline()
    {
        // Check the folder is existed or not. Existed -> save the file to the folder. Not existed -> create a folder and save the file.
        CheckFolderExists(timelinePath);

        TimelineData newTimeline = ScriptableObject.CreateInstance<TimelineData>();
        AssetDatabase.CreateAsset(newTimeline, AssetDatabase.GenerateUniqueAssetPath(Path.Combine(timelinePath, "Timeline.asset")));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    // Used to generate file name for new objects
    private static string NextObjectName(ObjectType scriptableObject)
    {
        switch (scriptableObject)
        {
            case ObjectType.sceneType:
                // Get all files in the target folder
                string[] sceneFiles = Directory.GetFiles(scenePath, "Scene*.unity");

                // Find the highest scene number
                int maxSceneNumber = 0;
                foreach (string file in sceneFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (fileName.StartsWith("Scene"))
                    {
                        if (int.TryParse(fileName.Substring(5), out int number))
                        {
                            if (number > maxSceneNumber)
                            {
                                maxSceneNumber = number;
                            }
                        }
                    }
                }
                return "Scene" + (maxSceneNumber + 1);

            case ObjectType.sessionType:
                // Get all files in the target folder
                string[] sessionFiles = Directory.GetFiles(sessionPath, "Session*.asset");

                // Find the highest session number
                int maxSessionNumber = 0;
                foreach (string file in sessionFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (fileName.StartsWith("Session"))
                    {
                        if (int.TryParse(fileName.Substring(7), out int number))
                        {
                            if (number > maxSessionNumber)
                            {
                                maxSessionNumber = number;
                            }
                        }
                    }
                }
                return "Session" + (maxSessionNumber + 1);

            case ObjectType.transitionType:
                // Get all files in the target folder
                string[] transitionFiles = Directory.GetFiles(transitionPath, "Transition*.asset");

                // Find the highest session number
                int maxTransitionNumber = 0;
                foreach (string file in transitionFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (fileName.StartsWith("Transition"))
                    {
                        if (int.TryParse(fileName.Substring(10), out int number))
                        {
                            if (number > maxTransitionNumber)
                            {
                                maxTransitionNumber = number;
                            }
                        }
                    }
                }
                return "Transition" + (maxTransitionNumber + 1);

            default:
                return "Invalid Input";
        }
    }

    // Used to check folders existed or not. If not create a new folder to store files.
    private static void CheckFolderExists(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath) || AssetDatabase.IsValidFolder(folderPath))
        {
            return;
        }

        // Get the parent folder path
        string parentFolder = Path.GetDirectoryName(folderPath);

        // If parent folder is not created, recursive in the parent folder and create a new folder.
        CheckFolderExists(parentFolder);

        // Create the required folder
        string newFoldername = Path.GetFileName(folderPath);
        AssetDatabase.CreateFolder(parentFolder, newFoldername);
    }
}

#endif
