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
        lineRenderer.positionCount = 2; // �������� �������� 2���� ��ġ ����
    }

    private void Update()
    {
        // �������� ������ ��ġ ����
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);

        // ���� �߻�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchLine();
        }
    }

    private void LaunchLine()
    {
        // ������ ����� �Ÿ� ���
        Vector3 direction = (endPoint.position - startPoint.position).normalized;
        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        // ���� �̵�
        StartCoroutine(MoveLine(startPoint.position, direction, distance));
    }

    private IEnumerator MoveLine(Vector3 startPosition, Vector3 direction, float distance)
    {
        float elapsedTime = 0f;
        Vector3 currentPos = startPosition;

        while (elapsedTime < distance / launchSpeed)
        {
            elapsedTime += Time.deltaTime;

            // ������ �̵���Ű�� ���� ���� ��ġ ������Ʈ
            currentPos = startPosition + direction * elapsedTime * launchSpeed;

            // ���� �������� ��ġ ������Ʈ
            lineRenderer.SetPosition(0, currentPos);

            yield return null;
        }

        // ������ ��ġ�� ������ ������Ŵ
        lineRenderer.SetPosition(0, endPoint.position);
    }
}