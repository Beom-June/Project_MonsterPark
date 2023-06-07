using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineArea : MonoBehaviour
{
    private LineUpController _lineUpController; // LineUpController 스크립트의 참조를 저장하기 위한 변수

    private void Start()
    {
        _lineUpController = GetComponentInParent<LineUpController>(); // 부모에 있는 LineUpController 스크립트의 참조를 가져옴
    }
    // 충돌시 NPC를 체크함
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("NPC"))
        {
            // 부딪힌 대상을 자식으로 추가
            collider.transform.parent = transform;
            // NPC의 위치를 (0, 0, 0)으로 설정
            // transform.localPosition = Vector3.zero;
            collider.transform.position = transform.position;
            // NPC가 LineUpController를 부모로 설정
            _lineUpController.CheckLineAreaChildren();
        }
    }
    // 자식이 제거될 때 호출됨
    private void OnTransformChildrenChanged()
    {
        // 자식이 변경되었을 때 디버그 출력
        Debug.Log("LineArea에 자식이 변경됨");

        // 자식이 삭제되었는지 체크
        List<Transform> childrenToMove = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("NPC"))
            {
                childrenToMove.Add(child);
                Debug.Log("34123213123s");
            }
        }

        // 자식을 다른 부모의 자식으로 이동
        foreach (Transform child in childrenToMove)
        {
                Debug.Log("55555555");
            // 이전 부모에서 제거
            child.SetParent(null);

            // 다른 부모의 자식으로 이동
            Transform targetParent = _lineUpController.FindAvailableLineArea();
            if (targetParent != null)
            {
                Debug.Log("fffdsfasfas");
                child.SetParent(targetParent);
                child.localPosition = Vector3.zero;

                // NPC가 LineUpController를 부모로 설정
                _lineUpController.CheckLineAreaChildren();
            }
        }
    }
}
