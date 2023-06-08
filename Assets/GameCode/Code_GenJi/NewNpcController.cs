using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNpcController : MonoBehaviour
{
    [SerializeField] private float speed; // 이동 속도

    private Animator animator;
    private Rigidbody rigidbody;
    private Transform currentWaypoint; // 현재 도달한 웨이포인트
    private bool isWaiting; // 대기 중인지 여부
    private float waitTime = 10.0f; // 대기 시간

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

                // 큐에서 웨이포인트를 가져오지 못한 경우 NPC 파괴
                Destroy(gameObject);
                return;
        }

        // NPC 이동
        Vector3 targetPosition = new Vector3(currentWaypoint.position.x, transform.position.y, currentWaypoint.position.z);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        Vector3 newVelocity = moveDirection * speed;
        rigidbody.velocity = newVelocity;

        // NPC 회전
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        // NPC 애니메이션 설정
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
