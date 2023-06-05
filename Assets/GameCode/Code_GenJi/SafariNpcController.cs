using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    private static System.Random _random = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
public class SafariNpcController : MonoBehaviour
{
    [SerializeField] private List<Transform> _wayPoints;                //  Safari Npc가 움직이는 경로
    [SerializeField] private float _speed;                              //  이동 속도

    private Animator _AnimatorSafariNpc;
    private Rigidbody _rigidSafariNpc;
    private int _waypointIndex;                                         //  waypoint index 값 저장
    private List<int> _randomIndices = new List<int>();                                   //  첫 번째 포인트에서 방문할 랜덤한 인덱스들을 저장하는 리스트

    private int _visitedCount;                                           //  방문한 중간 지점의 개수

    void Start()
    {
        _AnimatorSafariNpc = GetComponent<Animator>();
        _rigidSafariNpc = GetComponent<Rigidbody>();
    }

    void Update()
    {
        SafariNpcWaypoint();
    }

    private void SafariNpcWaypoint()
    {

        Vector3 _destination = _wayPoints[_waypointIndex].transform.position;
        Vector3 _newPos = Vector3.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
        transform.position = _newPos;
        _AnimatorSafariNpc.SetFloat("isWalk", 1f);

        // NPC가 이동시 바라보는 방향
        Vector3 _lookDirection = _destination - transform.position;
        _lookDirection.y = 0f; // 수직 방향은 고려하지 않음
        if (_lookDirection != Vector3.zero)
        {
            Quaternion _lookRotation = Quaternion.LookRotation(_lookDirection);
            transform.rotation = _lookRotation;
        }


        float _distance = Vector3.Distance(transform.position, _destination);
        if (_distance <= 0.05f)
        {
            // 종료 지점 도착시 삭제
            if (_waypointIndex == _wayPoints.Count - 1)
            {
                Destroy(this.gameObject);
                return;
            }
            else if (_waypointIndex == 0)
            {
                // 첫 번째 포인트에 도착한 경우
                _waypointIndex++;
            }
            else if (_waypointIndex < _wayPoints.Count - 1)
            {
                // 중간 지점에 도착한 경우
                if (_visitedCount < 4)
                {
                    // 최대 4곳의 중간 지점을 랜덤으로 방문
                    List<int> randomIndices = GetRandomIndices();
                    _waypointIndex = randomIndices[_visitedCount];
                    _visitedCount++;
                }
                else
                {
                    // 이미 2곳의 중간 지점을 방문한 경우, 마지막 지점으로 이동
                    _waypointIndex = _wayPoints.Count - 1;
                }
            }
        }
    }
    private List<int> GetRandomIndices()
    {
        List<int> randomIndices = new List<int>();

        for (int i = 1; i < _wayPoints.Count - 1; i++)
        {
            randomIndices.Add(i);
        }

        randomIndices.Shuffle();
        return randomIndices;
    }
}
