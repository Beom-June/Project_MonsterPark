using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLauncher : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float launchSpeed = 10f;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // 시작점과 끝점으로 2개의 위치 지정
    }

    private void Update()
    {
        // 시작점과 끝점의 위치 설정
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);

        // 라인 발사
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchLine();
        }
    }

    private void LaunchLine()
    {
        // 라인의 방향과 거리 계산
        Vector3 direction = (endPoint.position - startPoint.position).normalized;
        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        // 라인 이동
        StartCoroutine(MoveLine(startPoint.position, direction, distance));
    }

    private IEnumerator MoveLine(Vector3 startPosition, Vector3 direction, float distance)
    {
        float elapsedTime = 0f;
        Vector3 currentPos = startPosition;

        while (elapsedTime < distance / launchSpeed)
        {
            elapsedTime += Time.deltaTime;

            // 라인을 이동시키기 위해 현재 위치 업데이트
            currentPos = startPosition + direction * elapsedTime * launchSpeed;

            // 라인 렌더러의 위치 업데이트
            lineRenderer.SetPosition(0, currentPos);

            yield return null;
        }

        // 도달한 위치에 라인을 고정시킴
        lineRenderer.SetPosition(0, endPoint.position);
    }
}