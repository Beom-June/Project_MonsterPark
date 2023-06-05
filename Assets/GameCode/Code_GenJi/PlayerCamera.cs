using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform _targetPlayer;           // 따라갈 플레이어
    private Vector3 _initialOffset;                     // 초기 오프셋 저장 벡터 저장
    private Quaternion _initialRotation;                // 초기 로테이션 값 저장

    private void Start()
    {
        // 초기 오프셋과 로테이션 값을 저장
        _initialOffset = transform.position - _targetPlayer.position;
        _initialRotation = transform.rotation;
    }
    private void LateUpdate()
    {
        if (_targetPlayer != null)
        {
            // 플레이어 위치에 초기 오프셋을 더하여 카메라 위치를 설정
            Vector3 _targetPosition = _targetPlayer.position + _initialOffset;
            transform.position = _targetPosition;
            // 초기 로테이션 값을 유지
            transform.rotation = _initialRotation;
        }
    }

}
