using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster;
public class FenceController : MonoBehaviour
{
    [SerializeField] private MonsterKind monsterKind;
    [SerializeField] private Transform[] randomTr = new Transform[9];

    private List<GameObject> monObj = new List<GameObject>();

    private int randIdx;
    private int monCount_inCage = 0;

    public Transform GetRandDest()
    {
        randIdx = Random.Range(0, 9);
        return randomTr[randIdx];
    }


    public MonsterKind GetMonsState()
    {
        return monsterKind;
    }

    public void AddMonCount(GameObject _monObj)
    {
        monCount_inCage++;
        Debug.Log($"몬스터의 수 {monCount_inCage}");
        monObj.Add(_monObj);
    }

    public GameObject GetMonObj()
    {
        return monObj[0];
    }

    public void SubMonCount()
    {
        monCount_inCage--;
    }

    public int GetMonCount()
    {
        return monCount_inCage;
    }
}
