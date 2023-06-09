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
    [SerializeField] private List<Transform> _wayPoints;                        //  Safari Npc가 움직이는 경로
    [SerializeField] private float _speed;                            //  이동 속도
    private Animator _animatorSafariNpc;
    private Rigidbody _rigidSafariNpc;

    // 웨이포인트 순항시 필요
    private int _currentWaypointIndex;             //  현재 도달한 웨이포인트의 인덱스를 저장
    private int _visitedCount;                                                  //  방문한 중간 지점의 개수
    [SerializeField] private float _waitTime = 10.0f;                           //  대기 시간
    [SerializeField] private bool _isWaiting;                                                    //  대기 중인지 여부
    [SerializeField] private float _waitTimer;                                                   //  대기 타이머

    // 줄서기 관련
    [SerializeField] public GameObject _moneyPrefab;
    [SerializeField] private bool _reachedFirstChild;                           //  0번째 인덱스의 자식 오브젝트에 도달했는지 여부를 나타내는 변수
    LineArea _lineArea;
    private Transform _destination;                                             //  목적지를 저장하는 변수
    private bool _isStarted = false; // 시작 여부를 나타내는 변수


    #region Property
    public bool _isNpcWaitingTime
    {
        get { return _isWaiting; }
        set { _isWaiting = value; }
    }

    public float _WaitTime
    {
        get => _waitTime;
    }

    public float _WaitTimer
    {
        set { _waitTimer = value; }
    }
    #endregion

    void Start()
    {
        _animatorSafariNpc = GetComponent<Animator>();
        _rigidSafariNpc = GetComponent<Rigidbody>();
        _lineArea = FindObjectOfType<LineArea>();


        // StartCoroutine(WaitAtFirstWaypoint(_waitTime));
        // _currentWaypointIndex를 0으로 초기화 (순서대로 안가는 현상이 일어남 지금)
        _currentWaypointIndex = 0;
    }

    void Update()
    {
        if (!_isStarted && Input.GetKeyDown(KeyCode.Space))
        {
            _isStarted = true; // 스페이스바를 누르면 시작
        }

        if (_isStarted)
        {
            SafariNpcWaypoint();
        }
    }

    private void SafariNpcWaypoint()
    {
        if (_currentWaypointIndex >= _wayPoints.Count)
        {
            Destroy(gameObject);
            return;
        }

        // 웨이팅 카운트 세는 곳
        if (_isWaiting)
        {
            _animatorSafariNpc.SetFloat("isWalk", 0f);

            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                // _currentWaypointIndex++;
                _isWaiting = false;
                _animatorSafariNpc.SetFloat("isWalk", 1f);
            }
            return;
        }

        // if (_currentWaypointIndex == 0 && !_reachedFirstChild)
        // {
        //     Transform firstChild = GetFirstChildWithNoChildren(_wayPoints[_currentWaypointIndex]);
        //     if (firstChild != null)
        //     {
        //         _destination = firstChild;
        //         if (transform.position == _destination.position)
        //         {
        //             _reachedFirstChild = true;
        //             _isWaiting = true;
        //         }
        //     }
        // }
        // else
        // {
        //     // 1번째 인덱스부터 순서대로 이동
        //     _destination = _wayPoints[_currentWaypointIndex];
        // }

        // 수정함 (영상용 코드)
        _destination = _wayPoints[_currentWaypointIndex];

        Vector3 newPos = Vector3.MoveTowards(transform.position, _destination.position, _speed * Time.deltaTime);
        transform.position = newPos;
        _animatorSafariNpc.SetFloat("isWalk", 1f);

        // NPC가 이동 시 바라보는 방향
        Vector3 lookDirection = _destination.position - transform.position;
        lookDirection.y = 0f; // 수직 방향은 고려하지 않음
        if (lookDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = lookRotation;
        }
        // 수정함 (영상용 코드)
        float distance = Vector3.Distance(transform.position, _destination.position);
        if (distance <= 0.05f)
        {
            _isWaiting = true;
            _waitTimer = _waitTime;
            _currentWaypointIndex++;

            //제스쳐 애니메이션 랜덤으로
            // RandAnim();
        }
        if (_currentWaypointIndex <= _wayPoints.Count)
        {
            _animatorSafariNpc.SetFloat("isWalk", _isWaiting ? 0f : 1f);
        }

        // float distance = Vector3.Distance(transform.position, _destination.position);
        // if (distance <= 0.05f)
        // {
        //     if (_currentWaypointIndex == 0)
        //     {
        //         Debug.Log("여기서 받아옴");
        //         _isWaiting = true;
        //         _waitTimer = _waitTime;
        //     }
        //      if (_currentWaypointIndex < _wayPoints.Count - 1)
        //     {
        //         if (_visitedCount < 9)
        //         {
        //             // 다음 포인트 가면 여기로 들어옴 지금
        //             Debug.Log("꼬임 방지 디버그====>");
        //             _isWaiting = true;
        //             _waitTimer = _waitTime;
        //             SpawnMoney();
        //             // List<int> randomIndices = GetRandomIndices();
        //             // _currentWaypointIndex = randomIndices[_visitedCount];
        //             _visitedCount++;
        //         }
        //         else
        //         {

        //             _currentWaypointIndex = _wayPoints.Count - 1;
        //         }
        //     }

        // }
    }
    private Transform GetFirstChildWithNoChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            LineArea lineArea = child.GetComponent<LineArea>();
            if (lineArea != null && !lineArea.HasChildren())
            {
                return child;
            }
        }
        return null;
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

    void SpawnMoney()
    {
        float randomValue = UnityEngine.Random.value;

        if (randomValue <= 0.5f)
        {
            Instantiate(_moneyPrefab, transform.position, Quaternion.Euler(-90f, 0f, 90f));
        }
    }
    void RandAnim()
    {
        int randomIndex = UnityEngine.Random.Range(0, 2); // 0 또는 1 중에서 랜덤한 값을 얻습니다.

        if (randomIndex == 0)
        {
            _animatorSafariNpc.SetTrigger("isThink"); // 첫 번째 setBool 실행
        }
        else
            _animatorSafariNpc.SetTrigger("isCheering"); // 두 번째 setBool 비활성화
    }

    public IEnumerator WaitAtFirstWaypoint(float _time)
    {
        yield return new WaitForSeconds(_time);
        _currentWaypointIndex++;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("PointInHouse"))
        {
            SpawnMoney();
            RandAnim();
        }
    }
}