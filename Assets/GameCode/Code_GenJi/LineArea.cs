using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineArea : MonoBehaviour
{
    private LineUpController _lineUpController; // LineUpController 스크립트의 참조를 저장하기 위한 변수
    [SerializeField] private bool _hasChild = false; // 자식 여부를 나타내는 변수

    #region Property
    public bool _hasChildren
    {
        get => _hasChild;
    }
    #endregion


    private void Start()
    {
        _lineUpController = GetComponentInParent<LineUpController>(); // 부모에 있는 LineUpController 스크립트의 참조를 가져옴
    }
    // 충돌시 NPC를 체크함
    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("NPC") && !_hasChild)
        {
            // 부딪힌 대상을 자식으로 추가
            collider.transform.SetParent(transform, true); // 자식으로 추가하고 부모의 변환도 따라가도록 설정
            collider.transform.localPosition = Vector3.zero; // 로컬 포지션을 (0, 0, 0)으로 설정

            // 자식이 추가되었으므로 true로 설정
            _hasChild = true;
        }
    }
    // 충돌이 끝났을 때 호출됨
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("NPC") && _hasChild)
        {
            // 부딪힌 대상의 부모를 해제
            collider.transform.SetParent(null, true); // 부모 해제하고 월드 좌표계로 변환도 따라가도록 설정

            // 자식이 제거되었으므로 false로 설정
            _hasChild = false;

            // 자식이 제거되었음을 LineUpController에 알림
            _lineUpController.CheckLineAreaChildren();
        }
    }

    // 자식이 제거될 때 호출됨
    private void OnTransformChildrenChanged()
    {
        // Debug.Log("LineArea에 자식이 변경됨");

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
