using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpController : MonoBehaviour
{
    [SerializeField] private List<Transform> _lineArea;             //  NPC가 줄 서는 위치
    void Start()
    {
        // LineUpNPCs();
    }

    void Update()
    {
        // 플레이어가 특정 위치에 닿으면, NPC는 돈을 던지고 밖으로 향함
        // 플레이어와 충돌 감지 로직 등을 구현해야 합니다.


        // LineArea에 자식이 추가되었는지 체크
        // bool hasChildren = CheckLineAreaChildren();
        // if (hasChildren)
        // {
        //     Debug.Log("ffff");
        //     // LineArea에 자식이 추가되었음을 처리하는 로직을 추가합니다.
        // }
        CheckLineAreaChildren();
    }

    private void LineUpNPC()
    {
        // 각 하위 트랜스폼에 순서대로 줄섬
        for (int i = 0; i < _lineArea.Count; i++)
        {
            if (_lineArea[i] != null)
            {
                // NPC를 _lineArea 리스트에 있는 위치로 이동시킵니다.
                _lineArea[i].position = transform.position + new Vector3(i * 2, 0, 0);

                // NPC가 LineUpController를 부모로 설정합니다.
                _lineArea[i].parent = transform;
            }
        }
    }
    public void CheckLineAreaChildren()
    {
        foreach (Transform lineAreaTransform in _lineArea)
        {
            if (lineAreaTransform.childCount > 0)
            {
                // LineArea에 자식이 추가되었음을 감지
                Debug.Log("LineArea에 자식이 추가");

                // 이전 LineArea에서 NPC를 가져와서 현재 LineArea로 이동시킴
                Transform childNPC = lineAreaTransform.GetChild(0); // 첫 번째 자식 NPC 가져오기
                childNPC.position = lineAreaTransform.position; // NPC를 LineArea 위치로 이동
                childNPC.parent = lineAreaTransform; // NPC의 부모를 LineArea로 설정
            }
        }
    }
    public Transform FindAvailableLineArea()
    {
        foreach (Transform lineAreaTransform in _lineArea)
        {
            if (lineAreaTransform != transform && lineAreaTransform.childCount == 0)
            {
                return lineAreaTransform;
            }
        }
        return null;
    }
}
