using System;
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
    [SerializeField] private List<Transform> _wayPoints;  // Safari Npc가 움직이는 경로
    [SerializeField] private float _speed;  // 이동 속도

    private Animator _animatorSafariNpc;
    private Rigidbody _rigidSafariNpc;
    private int _currentWaypointIndex;  // 현재 도달한 웨이포인트의 인덱스를 저장
    private List<int> _randomIndices = new List<int>();  // 첫 번째 포인트에서 방문할 랜덤한 인덱스들을 저장하는 리스트
    private int _visitedCount;  // 방문한 중간 지점의 개수

    private SafariNpcController _previousNpc;  // 이전 NPC

    [SerializeField] private float _waitTime = 3.0f;  // 대기 시간
    private bool _isWaiting;  // 대기 중인지 여부
    private float _waitTimer;  // 대기 타이머



    [SerializeField]private bool _istest = false;
    void Start()
    {
        _animatorSafariNpc = GetComponent<Animator>();
        _rigidSafariNpc = GetComponent<Rigidbody>();

        // 이전 NPC 가져오기
        SafariNpcController[] npcControllers = FindObjectsOfType<SafariNpcController>();
        int index = Array.IndexOf(npcControllers, this);
        if (index > 0)
        {
            _previousNpc = npcControllers[index - 1];
        }
    }

    void Update()
    {
        // SafariNpcWaypoint();
        if (Input.GetMouseButtonUp(0))
        {
            // if (_istest)
                // NPC 삭제 로직을 추가하세요.
                // 예를 들어, 해당 NPC 오브젝트를 제거하거나 비활성화할 수 있습니다.
                // 이후에 OnTransformChildrenChanged 함수가 호출됩니다.
                //  gameObject.SetActive(false);
        }
    }

    private void SafariNpcWaypoint()
    {
        // 첫 번째 NPC가 멈추고 있으면 대기
        if (_currentWaypointIndex == 0 && _previousNpc != null && !_previousNpc.IsMoving())
        {
            _animatorSafariNpc.SetFloat("isWalk", 0f);

            if (!_isWaiting)
            {
                _isWaiting = true;
                _waitTimer = _waitTime;
            }
            else
            {
                _waitTimer -= Time.deltaTime;
                if (_waitTimer <= 0f)
                {
                    _currentWaypointIndex++;
                    _isWaiting = false;
                }
            }
            return;
        }

        Vector3 destination = _wayPoints[_currentWaypointIndex].transform.position;
        Vector3 newPos = Vector3.MoveTowards(transform.position, destination, _speed * Time.deltaTime);
        transform.position = newPos;
        _animatorSafariNpc.SetFloat("isWalk", 1f);

        // NPC가 이동 시 바라보는 방향
        Vector3 lookDirection = destination - transform.position;
        lookDirection.y = 0f; // 수직 방향은 고려하지 않음
        if (lookDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = lookRotation;
        }

        float distance = Vector3.Distance(transform.position, destination);
        if (distance <= 0.05f)
        {
            if (_currentWaypointIndex == _wayPoints.Count - 1)
            {
                Destroy(gameObject);
                return;
            }
            else if (_currentWaypointIndex == 0)
            {
                _currentWaypointIndex++;
            }
            else if (_currentWaypointIndex < _wayPoints.Count - 1)
            {
                if (_visitedCount < 4)
                {
                    List<int> randomIndices = GetRandomIndices();
                    _currentWaypointIndex = randomIndices[_visitedCount];
                    _visitedCount++;
                }
                else
                {
                    _currentWaypointIndex = _wayPoints.Count - 1;
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

    public bool IsMoving()
    {
        return _animatorSafariNpc.GetFloat("isWalk") > 0f;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("LineArea"))
        {
        }
    }
}