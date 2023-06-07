using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerController : MonoBehaviour
{

    [SerializeField] private Transform _outPoint;                       //  퇴장하는 위치값 
    [SerializeField] private bool _isExitScene = false;                 //  돈을 던지고, 플레이어가 돈을 먹으면 이 Bool 값은 True 됨
    [SerializeField] private float _moveSpeed = 5.0f;                   //  해당 NPC 이동 속도

    private Quaternion _targetRotation;                                 //  목표 회전값
    private Animator _animBuyer;

    #region Property
    public bool isExit
    {
        set { _isExitScene = value; }
    }
    #endregion

    void Start()
    {
        _animBuyer = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isExitScene)
        {
            // 퇴장
            MoveToExitPoint();
        }
    }
    private void MoveToExitPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, _outPoint.position, _moveSpeed * Time.deltaTime);
        _animBuyer.SetFloat("isWalk", 1f);

        // 움직이는 방향으로 쳐다보게함
        Vector3 direction = (_outPoint.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, 10f * Time.deltaTime);
        }

        // 도착 지점에 도달하면?
        if (transform.position == _outPoint.position)
        {
            // 예: 플레이어를 비활성화하거나 삭제
            //gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}
