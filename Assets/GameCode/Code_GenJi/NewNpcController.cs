using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNpcController : MonoBehaviour
{
    [SerializeField] private float speed; // �̵� �ӵ�

    private Animator animator;
    private Rigidbody rigidbody;
    private Transform currentWaypoint; // ���� ������ ��������Ʈ
    private bool isWaiting; // ��� ������ ����
    private float waitTime = 10.0f; // ��� �ð�

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isWaiting)
        {
            MoveToWaypoint();
        }
    }

    private void MoveToWaypoint()
    {
        if (currentWaypoint == null)
        {

                // ť���� ��������Ʈ�� �������� ���� ��� NPC �ı�
                Destroy(gameObject);
                return;
        }

        // NPC �̵�
        Vector3 targetPosition = new Vector3(currentWaypoint.position.x, transform.position.y, currentWaypoint.position.z);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        Vector3 newVelocity = moveDirection * speed;
        rigidbody.velocity = newVelocity;

        // NPC ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        // NPC �ִϸ��̼� ����
        animator.SetFloat("isWalk", 1f);
    }

    public IEnumerator WaitAtWaypoint(float time)
    {
        isWaiting = true;
        yield return new WaitForSeconds(time);
        isWaiting = false;
        currentWaypoint = null;
        animator.SetFloat("isWalk", 0f);
    }
}
