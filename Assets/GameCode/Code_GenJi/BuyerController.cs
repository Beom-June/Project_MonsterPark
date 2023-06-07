using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerController : MonoBehaviour
{

    [SerializeField] private Transform _outPoint;                       //  �����ϴ� ��ġ�� 
    [SerializeField] private bool _isExitScene = false;                 //  ���� ������, �÷��̾ ���� ������ �� Bool ���� True ��
    [SerializeField] private float _moveSpeed = 5.0f;                   //  �ش� NPC �̵� �ӵ�

    private Quaternion _targetRotation;                                 //  ��ǥ ȸ����
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
            // ����
            MoveToExitPoint();
        }
    }
    private void MoveToExitPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, _outPoint.position, _moveSpeed * Time.deltaTime);
        _animBuyer.SetFloat("isWalk", 1f);

        // �����̴� �������� �Ĵٺ�����
        Vector3 direction = (_outPoint.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, 10f * Time.deltaTime);
        }

        // ���� ������ �����ϸ�?
        if (transform.position == _outPoint.position)
        {
            // ��: �÷��̾ ��Ȱ��ȭ�ϰų� ����
            //gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}
