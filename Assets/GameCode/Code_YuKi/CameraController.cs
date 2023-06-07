using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraData
{
    public Transform transform;
    public Vector3 offset;
    public float speed;
    [HideInInspector]
    public Vector3 velocity = Vector3.zero;
}

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _targetPlayer;           // 따라갈 플레이어
    private Vector3 _initialOffset;                     // 초기 오프셋 저장 벡터 저장
    private Quaternion _initialRotation;                // 초기 로테이션 값 저장

    public bool isMove = false;
    private void Start()
    {
        // 초기 오프셋과 로테이션 값을 저장
        _initialOffset = transform.position - _targetPlayer.position;
        _initialRotation = transform.rotation;
    }
    private void LateUpdate()
    {
        if (_targetPlayer != null && !isMove)
        {
            // 플레이어 위치에 초기 오프셋을 더하여 카메라 위치를 설정
            Vector3 _targetPosition = _targetPlayer.position + _initialOffset;
            transform.position = _targetPosition;
            // 초기 로테이션 값을 유지
            transform.rotation = _initialRotation;
        }
    }

    public IEnumerator MoveCam(Camera newCamera)
    {
        float distanceThreshold = 0.01f;

        while (Vector3.Distance(transform.position, newCamera.transform.position) > distanceThreshold)
        {
            var orginPos = transform.position;
            var originRot = transform.rotation;
            var velocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(orginPos, newCamera.transform.position, ref velocity, 0.25f);
            
            yield return null;
        }

        isMove = false;
    }
}
