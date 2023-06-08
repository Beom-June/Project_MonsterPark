using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using UnityEngine.AI;


public class Customer : MonoBehaviour
{


    private bool hasReachedCounter = false;
    private Money npcBezierCurve;
    private bool isPay = false; // 돈을 다 지불했다면 
    PlayerManager pm;


    [SerializeField] private Transform _outPoint;                       //  �����ϴ� ��ġ�� 
    [SerializeField] private bool _isExitScene = false;                 //  ���� ������, �÷��̾ ���� ������ �� Bool ���� True ��
    [SerializeField] private float _moveSpeed = 5.0f;                   //  �ش� NPC �̵� �ӵ�

    int money = 80; // 8장 - 80원. 9장 10장 11장 
    private Quaternion _targetRotation;                                 //  ��ǥ ȸ����
    private Animator _animBuyer;


    private void Start()
    {
        _animBuyer = GetComponent<Animator>();

        npcBezierCurve = GetComponent<Money>();
        pm = PlayerManager.instance;
    }

    void Update()
    {
        // 플레이어 상태 업데이트
        if (pm.isInCounter && !isPay)
        {
            npcBezierCurve.boolThrow = true;
            isPay = true;
        }
        else if (!pm.isInCounter && isPay)
        {
            MoveToExitPoint();
        }               
    }

    private void MoveToExitPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, _outPoint.position, _moveSpeed * Time.deltaTime);
        _animBuyer.SetFloat(AnimType.isWalk.ToString(), 1f);

        Vector3 direction = (_outPoint.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, 10f * Time.deltaTime);
        }

        if (transform.position == _outPoint.position)
        {
            Destroy(this.gameObject);
        }
    }

}
