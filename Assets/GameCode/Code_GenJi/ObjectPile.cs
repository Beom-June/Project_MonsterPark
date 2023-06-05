using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPile : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 오브젝트 프리팹
    public int rows = 3; // 행 수
    public int columns = 5; // 열 수
    public int maxObjects = 30; // 최대 오브젝트 수
    private int currentObjects = 0; // 현재 오브젝트 수
    public float objectSpacing = 1.0f; // 오브젝트간의 간격

    private System.Collections.IEnumerator GenerateObjects()
    {
        for (int level = 0; level < Mathf.CeilToInt((float)maxObjects / (rows * columns)); level++)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    Vector3 position = new Vector3(column / objectSpacing, level / objectSpacing , row / objectSpacing); // 오브젝트의 위치 설정
                    Instantiate(objectPrefab, position, Quaternion.Euler(-90f, 0, 0)); // 오브젝트 생성
                    currentObjects++;

                    if (currentObjects >= maxObjects)
                    {
                        yield break; // 최대 오브젝트 수에 도달하면 종료
                    }

                    yield return new WaitForSeconds(0.1f); // 각 오브젝트 생성 사이에 0.1초의 딜레이
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Money"))
        {
            StartCoroutine(GenerateObjects());
        }
    }
}
