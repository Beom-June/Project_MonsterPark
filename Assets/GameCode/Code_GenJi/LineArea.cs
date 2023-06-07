using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineArea : MonoBehaviour
{
    private LineUpController _lineUpController; // LineUpController 스크립트의 참조를 저장하기 위한 변수
    [SerializeField] private bool _hasChild = false; // 자식 여부를 나타내는 변수
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
            collider.transform.position = transform.position;

            // 자식이 추가되었으므로 true로 설정
            _hasChild = true;
        }
    }

    // 자식이 제거될 때 호출됨
    private void OnTransformChildrenChanged()
    {
        // 자식이 변경되었을 때 디버그 출력
        Debug.Log("LineArea에 자식이 변경됨");

        // 자식이 삭제되었는지 체크
        _hasChild = (transform.childCount > 0);

        // 자식 변경 여부에 따라 로직 수행
        if (!_hasChild)
        {
            // 자식이 없으므로 LineArea에 자식이 없음을 알림
            _lineUpController.CheckLineAreaChildren();
        }
    }

    // LineArea에 자식이 있는지 여부 반환
    public bool HasChildren()
    {
        return _hasChild;
    }
}
