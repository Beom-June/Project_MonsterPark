using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPile : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;                                                 //  생성할 오브젝트 프리팹
    [SerializeField] private int _rows = 3;                                                           //  행 수
    [SerializeField] private int _columns = 5;                                                        //  열 수
    [SerializeField] private int _maxObjects = 30;                                                    //  최대 오브젝트 수
    [SerializeField] private int _currentObjects = 0;                                                //  현재 오브젝트 수
    [SerializeField] private float _objectSpacing = 1.0f;                                             //  오브젝트간의 간격
    [SerializeField] private List<GameObject> spawnedObjects = new List<GameObject>(); // 생성된 오브젝트를 추적하기 위한 리스트
    private void Start()
    {
        //StartCoroutine(GenerateObjects());
    }
    private IEnumerator GenerateObjects(int _level)
    {
        //int totalLevels = Mathf.CeilToInt((float)maxObjects / (rows * columns)); // 총 레벨 수 계산

        //for (int level = 0; level < totalLevels; level++)
        //{
        //    for (int row = 0; row < rows; row++)
        //    {
        //        for (int column = 0; column < columns; column++)
        //        {
        //            Vector3 position = new Vector3(column / objectSpacing, level / objectSpacing / 2, row / objectSpacing); // 오브젝트의 위치 설정
        //            GameObject spawnedObject = Instantiate(objectPrefab, position, Quaternion.Euler(-90f, 0, 90)); // 오브젝트 생성
        //            spawnedObjects.Add(spawnedObject); // 생성된 오브젝트를 리스트에 추가
        //            currentObjects++;

        //            if (currentObjects >= maxObjects)
        //            {
        //                yield break; // 최대 오브젝트 수에 도달하면 종료
        //            }

        //            yield return new WaitForSeconds(0.1f); // 각 오브젝트 생성 사이에 0.1초의 딜레이
        //        }
        //    }
        //}
        int totalLevels = Mathf.CeilToInt((float)_maxObjects / (_rows * _columns)); // 총 레벨 수 계산

        for (int row = 0; row < _rows; row++)
        {
            for (int column = 0; column < _columns; column++)
            {
                Vector3 position = new Vector3(column / _objectSpacing, _level / _objectSpacing / 2, row / _objectSpacing); // 오브젝트의 위치 설정
                GameObject spawnedObject = Instantiate(objectPrefab, position, Quaternion.Euler(-90f, 0, 90)); // 오브젝트 생성
                spawnedObjects.Add(spawnedObject); // 생성된 오브젝트를 리스트에 추가
                _currentObjects++;

                if (_currentObjects >= _maxObjects)
                {
                    yield break; // 최대 오브젝트 수에 도달하면 종료
                }

                yield return new WaitForSeconds(0.1f); // 각 오브젝트 생성 사이에 0.1초의 딜레이
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Money"))
        {
            int _level = Mathf.CeilToInt((float)_currentObjects / (_rows * _columns)); // 현재 레벨 계산
            StartCoroutine(GenerateObjects(_level));
        }
    }
}
