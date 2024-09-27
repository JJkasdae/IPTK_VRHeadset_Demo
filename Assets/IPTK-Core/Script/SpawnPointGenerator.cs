#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnPointGenerator : MonoBehaviour
{
    public GameObject spawnPointPrefab;
    private List<Vector3> spawnPoints = new List<Vector3>();

    public List<Vector3> GetSpawnPoints()
    {
        return spawnPoints;
    }

    public void GenerateOrderedSpawnPoints(int numberOfPoints, GameObject floorPrefab, Vector3 startPosition, float scale, MovementType navigationType, PlayerType userType, string parentName)
    {
        GameObject parentObject = new GameObject(parentName);
        parentObject.transform.position = startPosition;

        // 实例化并调整floor的大小
        GameObject floor = PrefabUtility.InstantiatePrefab(floorPrefab) as GameObject;
        floor.transform.position = startPosition;
        floor.transform.localScale = new Vector3(scale, 1, scale);
        floor.transform.SetParent(parentObject.transform);

        // 创建一个空的父对象，用于包含所有生成的 spawn points
        GameObject spawnPointsParent = new GameObject("SpawnPoints");
        spawnPointsParent.transform.position = startPosition;
        spawnPointsParent.transform.SetParent(parentObject.transform);

        // 获取floor的尺寸信息
        Vector3 floorSize = floor.GetComponent<Renderer>().bounds.size;

        // 计算行和列的数量，使得生成点接近正方形排列
        int columns = Mathf.CeilToInt(Mathf.Sqrt(numberOfPoints));
        int rows = Mathf.CeilToInt((float)numberOfPoints / columns);

        // 计算生成点的间隔
        float spacingX = floorSize.x / columns;
        float spacingZ = floorSize.z / rows;

        // 根据设定的数量和顺序在floor平面上生成spawn points
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if ((i * columns + j) >= numberOfPoints)
                    break;

                Vector3 spawnPosition = new Vector3(
                    startPosition.x + (j * spacingX) - (floorSize.x / 2) + (spacingX / 2),
                    startPosition.y,
                    startPosition.z + (i * spacingZ) - (floorSize.z / 2) + (spacingZ / 2)
                );

                GameObject spawnPoint = PrefabUtility.InstantiatePrefab(spawnPointPrefab) as GameObject;
                spawnPoint.transform.position = spawnPosition;

                // 将生成的 spawn point 作为子对象添加到父对象中
                spawnPoint.transform.SetParent(spawnPointsParent.transform);

                // Set movement type
                SpawnPointAttribute spawnPointAttribute = spawnPoint.GetComponent<SpawnPointAttribute>();
                if (spawnPointAttribute != null)
                {
                    spawnPointAttribute.SetMovementType(navigationType);
                    spawnPointAttribute.SetPlayerType(userType);
                }

                // Add a new spawn point in the list
                spawnPoints.Add(spawnPosition);
            }
        }

        Debug.Log(spawnPoints.Count);
    }
}
#endif