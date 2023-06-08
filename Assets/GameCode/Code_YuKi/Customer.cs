using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Customer : MonoBehaviour
{


    private bool hasReachedCounter = false;
    private Money npcBezierCurve;
    private bool isPay = false; // 돈을 다 지불했다면 
    PlayerManager pm;
    public CustomerStateType customerState = CustomerStateType.Entry;
    public CustomerStateType beforeCustomerState = CustomerStateType.Disable;

    
    private Transform _movePoint;                       //  �����ϴ� ��ġ�� 
    [SerializeField] private Transform _fence;                       //  �����ϴ� ��ġ�� 
    [SerializeField] private Transform _counter;                       //  �����ϴ� ��ġ�� 
    [SerializeField] private Transform _outPoint;                       //  �����ϴ� ��ġ�� 

    [SerializeField] private bool _isExitScene = false;                 //  ���� ������, �÷��̾ ���� ������ �� Bool ���� True ��
    [SerializeField] private float _moveSpeed = 5.0f;                   //  �ش� NPC �̵� �ӵ�

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
        switch (customerState)
        {
            case CustomerStateType.Entry:
                _movePoint = _fence;
                Move();
                break;
            case CustomerStateType.Watching:
                _animBuyer.SetBool(AnimType.isThink.ToString(), true);
                StartCoroutine(WaitAndTransition(Random.Range(3, 5), CustomerStateType.Standby));
                break;
            case CustomerStateType.Standby:
            
                _animBuyer.SetBool(AnimType.isThink.ToString(), false);
                _movePoint = _counter;
                Move();
                break;
            case CustomerStateType.Calculation:
                if (pm.isInCounter && !isPay)
                {
                    npcBezierCurve.boolThrow = true;
                    isPay = true;
                    StartCoroutine(WaitAndTransition(1f, CustomerStateType.Out));
                }
                break;
            case CustomerStateType.Out:
                _movePoint = _outPoint;
                Move();
                break;
        }
        

      
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _moveSpeed * Time.deltaTime);
        _animBuyer.SetFloat(AnimType.isWalk.ToString(), 1f);

        Vector3 direction = (_movePoint.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, 10f * Time.deltaTime);
        }

        if (transform.position == _fence.position)
        {
            customerState = CustomerStateType.Watching;
        }
        else if (customerState == CustomerStateType.Standby && Vector3.Distance(transform.position, _counter.position) < 0.1f)
        {
            _animBuyer.SetFloat(AnimType.isWalk.ToString(), 0f);
            customerState = CustomerStateType.Calculation;
        }
        else if (transform.position == _outPoint.position)
        {
            Destroy(this.gameObject);
        }
         
    }
    
    private IEnumerator WaitAndTransition(float waitTime, CustomerStateType nextCustomerState)
    {
        yield return new WaitForSeconds(waitTime);
        customerState = nextCustomerState;
    }

}
