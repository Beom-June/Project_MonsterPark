using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class moneyPointInfo
{
    // public Vector3 _pos;
    // public Vector3 _startPoint;
    // public Vector3 _endPoint;
    public Transform _pos;
    public Transform _startPoint;
    public Transform _endPoint;
}

public class MoneyBezierCurves : MonoBehaviour
{
    [SerializeField] private moneyPointInfo _moneyPoint;
    [SerializeField] private GameObject _moneyPrefab;
    [SerializeField] private bool _isThrow = false;
    [SerializeField] private bool _shouldThrow = false;

    #region Property
    public bool boolThrow
    {
        set
        {
            _isThrow = value;
            _shouldThrow = true;
        }
    }

    #endregion

    private void Start()
    {
        if (_isThrow)
            ThrowMoney();
    }

    private void Update()
    {
        if (_shouldThrow)
        {
            _shouldThrow = false;
            ThrowMoney();
        }
    }

    private void ThrowMoney()
    {
        // _moneyPrefab 생성
        GameObject moneyObject = Instantiate(_moneyPrefab, _moneyPoint._startPoint.position, Quaternion.Euler(-90.0f, 0, 0));

        // 아이템 이동 애니메이션 설정
        StartCoroutine(MoveItemWithBezierCurve(moneyObject.transform, _moneyPoint._startPoint.position, _moneyPoint._endPoint.position));
    }

    private IEnumerator MoveItemWithBezierCurve(Transform _itemTransform, Vector3 _startPoint, Vector3 _endPoint)
    {
        float _duration = 1f; // 이동에 걸리는 시간
        float _elapsed = 0f;

        while (_elapsed < _duration)
        {
            _elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsed / _duration); // 보간값 계산

            // 베지어 곡선 계산
            Vector3 position = CalculateBezierCurve(_startPoint, _moneyPoint._pos.position, _endPoint, t);

            _itemTransform.position = position;

            yield return null;
        }

        // 이동 완료 후 아이템 삭제
        Destroy(_itemTransform.gameObject);
    }

    private Vector3 CalculateBezierCurve(Vector3 _startPoint, Vector3 _controlPoint, Vector3 _endPoint, float t)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 _position = (uu * _startPoint) + (4f * u * t * _controlPoint) + (tt * _endPoint);

        return _position;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("MoneyPoint"))
        {
            ThrowMoney();
        }
    }
}
