using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SpawnpointsGeneration: EditorWindow
{
    private int numberOfParticipants;
    private int numberOfPresenters;
    private Vector3 startPositionOfPresenterFloor;
    private Vector3 startPositionOfAudienceFloor;
    private GameObject floorPrefab;
    private GameObject spawnPointPrefab;
    private float scaleOfPresenterFloor = 1.0f; // 单一scale值
    private float scaleOfAudienceFloor = 1.0f;
    private SpawnPointGenerator manager;
    private MovementType presenterNavigationType;
    private MovementType audienceNavigationType;
    private PlayerType presenterType = PlayerType.Presenter;
    private PlayerType audienceType = PlayerType.Audience;

    [MenuItem("ImmersivePresentation/Generate spawn points")]
    public static void ShowPopUpWindow()
    {
        GetWindow<SpawnpointsGeneration>("Spawn Point Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Spawn Point Generator", EditorStyles.boldLabel);

        // 输入生成点的数量
        //numberOfPoints = EditorGUILayout.IntField("Number of Spawn Points", numberOfPoints);

        numberOfPresenters = EditorGUILayout.IntField("Number of Presenters", numberOfPresenters);

        numberOfParticipants = EditorGUILayout.IntField("Number of Participants", numberOfParticipants);

        // 输入起始位置
        //startPosition = EditorGUILayout.Vector3Field("Initial Position of Presenter's Floor", startPosition);

        startPositionOfPresenterFloor = EditorGUILayout.Vector3Field("Initial Position of Presenter's Floor", startPositionOfPresenterFloor);

        startPositionOfAudienceFloor = EditorGUILayout.Vector3Field("Initial Position of Audience's Floor", startPositionOfAudienceFloor);

        // 输入floor和spawn point的Prefab
        floorPrefab = (GameObject)EditorGUILayout.ObjectField("Floor Prefab", floorPrefab, typeof(GameObject), false);
        spawnPointPrefab = (GameObject)EditorGUILayout.ObjectField("Spawn Point Prefab", spawnPointPrefab, typeof(GameObject), false);

        // 输入scale值
        scaleOfPresenterFloor = EditorGUILayout.FloatField("Scale of Presenter's Floor", scaleOfPresenterFloor);

        scaleOfAudienceFloor = EditorGUILayout.FloatField("Scale of Audience's Floor", scaleOfAudienceFloor);

        // Select movement type
        presenterNavigationType = (MovementType)EditorGUILayout.EnumPopup("Presenter Movement Type", presenterNavigationType);
        audienceNavigationType = (MovementType)EditorGUILayout.EnumPopup("Audience Movement Type", audienceNavigationType);

        // 生成按钮
        if (GUILayout.Button("Generate Spawn Points"))
        {
            GenerateSpawnPoints();
        }
    }

    private void GenerateSpawnPoints()
    {
        if (floorPrefab == null || spawnPointPrefab == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign both floor and spawn point prefabs.", "OK");
            return;
        }

        if (manager == null)
        {
            GameObject managerObject = new GameObject("SpawnPointManager");
            manager = managerObject.AddComponent<SpawnPointGenerator>();
        }

        manager.spawnPointPrefab = spawnPointPrefab;
        manager.GenerateOrderedSpawnPoints(numberOfPresenters, floorPrefab, startPositionOfPresenterFloor, scaleOfPresenterFloor, presenterNavigationType, presenterType, "Presenter Spawn Points");
        manager.GenerateOrderedSpawnPoints(numberOfParticipants, floorPrefab, startPositionOfAudienceFloor, scaleOfAudienceFloor, audienceNavigationType, audienceType, "Audience Spawn Points");
    }


}


