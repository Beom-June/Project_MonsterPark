using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster;
public class FenceController : MonoBehaviour
{
    [SerializeField] private MonsterKind monsterKind;
    [SerializeField] private Transform[] randomTr = new Transform[9];

    private int randIdx;
    private int monCount_inCage;

    public Transform GetRandDest()
    {
        randIdx = Random.Range(0, 9);
        return randomTr[randIdx];
    }


    public MonsterKind GetMonsState()
    {
        return monsterKind;
    }

    public void SetMonCount(int _monCnt)
    {
        monCount_inCage = _monCnt;
    }
}
