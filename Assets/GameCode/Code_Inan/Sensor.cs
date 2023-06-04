using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Sensor : MonoBehaviour
{
    public float detectionAngle = 45f;  // 감지 각도
    public float detectionRange = 5f;   // 감지 범위
    public LayerMask targetLayer;       // 감지할 레이어
    public float alphaColor = 0.3f;
    public Color color;

    Image img;
    GameObject imgObj;

    private MeshRenderer closestTargetRenderer;
    private Color closestTargetOriginalColor;

    private void Update()
    {
        // 부채꼴 모양의 감지
        Collider[] targets = Physics.OverlapSphere(transform.position, detectionRange, targetLayer);
        bool isTargetDetected = targets.Length > 0; // 타겟이 감지되었는지 여부 확인

        // 원래의 색으로 복원
        if (!isTargetDetected && closestTargetRenderer != null)
        {
            closestTargetRenderer.material.color = closestTargetOriginalColor;
            closestTargetRenderer = null;
            img.fillAmount = 1.0f;
            Debug.Log("원래 색 복원");
        }

        float closestDistance = Mathf.Infinity;
        foreach (Collider target in targets)
        {
            Vector3 directionToTarget = target.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            if (angle <= detectionAngle * 0.5f)
            {
                float distanceToTarget = directionToTarget.magnitude;

                // 가장 가까운 타겟의 색상 변경
                if (distanceToTarget < closestDistance)
                {
                    if (closestTargetRenderer != null)
                    {
                        closestTargetRenderer.material.color = closestTargetOriginalColor;
                    }
                    closestTargetRenderer = target.GetComponent<MeshRenderer>();
                    img = target.GetComponentInChildren<Image>();
                    img.fillAmount -= Time.deltaTime;
                    closestTargetOriginalColor = closestTargetRenderer.material.color;
                    closestTargetRenderer.material.color = Color.red;
                    closestDistance = distanceToTarget;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // 감지 범위를 시각적으로 표시
        Gizmos.color = color;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-detectionAngle * 0.5f, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(detectionAngle * 0.5f, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;

        Gizmos.DrawRay(transform.position, leftRayDirection * detectionRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * detectionRange);
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange);
        Gizmos.DrawRay(transform.position, -transform.forward * detectionRange);
        Gizmos.DrawRay(transform.position, transform.right * detectionRange);
        Gizmos.DrawRay(transform.position, -transform.right * detectionRange);
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 원하는 동작을 수행하거나 적색 부채꼴 메시를 그립니다.
        if (closestTargetRenderer != null)
        {
            int numSegments = 30;
            float angleStep = detectionAngle / numSegments;
            Quaternion startRotation = Quaternion.AngleAxis(-detectionAngle * 0.5f, Vector3.up);

            Vector3[] vertices = new Vector3[numSegments + 2];
            Color[] colors = new Color[numSegments + 2];
            int[] triangles = new int[numSegments * 3];

            vertices[0] = transform.position;
            colors[0] = color;

            for (int i = 0; i <= numSegments; i++)
            {
                Quaternion rotation = startRotation * Quaternion.Euler(0f, i * angleStep, 0f);
                Vector3 direction = rotation * transform.forward;
                vertices[i + 1] = transform.position + direction * detectionRange;
                colors[i + 1] = color;

                if (i < numSegments)
                {
                    int triangleIndex = i * 3;
                    triangles[triangleIndex] = 0;
                    triangles[triangleIndex + 1] = i + 1;
                    triangles[triangleIndex + 2] = i + 2;
                }
            }

            Mesh sensorMesh = new Mesh();
            sensorMesh.vertices = vertices;
            sensorMesh.colors = colors;
            sensorMesh.triangles = triangles;

            Graphics.DrawMesh(sensorMesh, Matrix4x4.identity, new Material(Shader.Find("Sprites/Default")), 0);
        }
    }
}
