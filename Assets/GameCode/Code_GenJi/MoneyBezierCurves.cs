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


    void ThrowMoney()
    {
        // _moneyPrefab 생성
        GameObject moneyObject = Instantiate(_moneyPrefab, _moneyPoint._startPoint.position, Quaternion.Euler(-90.0f, 0, 0));

        // 아이템 이동 애니메이션 설정
        StartCoroutine(MoveItemWithBezierCurve(moneyObject.transform, _moneyPoint._startPoint.position, _moneyPoint._endPoint.position));
    }

    private IEnumerator MoveItemWithBezierCurve(Transform itemTransform, Vector3 startPoint, Vector3 endPoint)
    {
        float duration = 1f; // 이동에 걸리는 시간
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration); // 보간값 계산

            // 베지어 곡선 계산
            Vector3 position = CalculateBezierCurve(startPoint, _moneyPoint._pos.position, endPoint, t);

            itemTransform.position = position;

            yield return null;
        }

        // 이동 완료 후 아이템 삭제
        Destroy(itemTransform.gameObject);
    }

    private Vector3 CalculateBezierCurve(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, float t)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 position = (uu * startPoint) + (2f * u * t * controlPoint) + (tt * endPoint);

        return position;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("MoneyPoint"))
        {
            ThrowMoney();
        }
    }
}
