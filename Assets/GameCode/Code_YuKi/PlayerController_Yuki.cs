using System;
using System.Collections;
using System.Collections.Generic;
using BrokenVector.LowPolyFencePack;
using UnityEngine;
using EnumTypes;

public class PlayerController_Yuki : MonoBehaviour
{

    public PlayerStateType playerState = PlayerStateType.None;
    public Money money;


     [Header("Player Settings")]
    [SerializeField] private float _moveSpeed = 5f;                //   이동 속도
    [SerializeField] private float _rotationSpeed = 5f;            //   이동 속도
    private float _horizontalAxis;                                  //  수평 입력 값
    private float _verticalAxis;                                    //  수직 입력 값
    private bool _isRun;                                            //  달리기 bool 값
    private Vector3 _moveVector;                                    //  moveVector 저장 
    private Animator _playerAnimator;                               //  Animator 저장
    private Rigidbody _playerRigidbody;                             //  Rigidbody 저장

    [SerializeField] private List<Money> npcBezierCurves = new List<Money>();

    private Money bezierCurves;


    #region instance
    public static PlayerController_Yuki Instance;
    private static PlayerController_Yuki instance
    {
        get
        {
            if (Instance != null) return Instance;
            if (PlayerController_Yuki.instance == null)
            {
                Debug.LogError("PlayerController_Yuki is null!");
            }
            Instance = PlayerController_Yuki.instance;
            return Instance;
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    
    #endregion

    private void Start()
    {
        money = GetComponent<Money>();

        _playerAnimator = GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();

        // 모든 NPC 찾기
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag(TagType.NPC.ToString());
        foreach (GameObject npcObject in npcObjects)
        {
            bezierCurves = npcObject.GetComponent<Money>();
            if (bezierCurves != null)
            {
                npcBezierCurves.Add(bezierCurves);
            }
        }
    }

    void Update()
    {
        PlayerInput();
        PlayerMove();
        PlayerTurn();
    }

     // Input
    private void PlayerInput()
    {
        _horizontalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");
        // 입력 값이 있는 경우에만 _isRun을 true로 설정
        _isRun = (_horizontalAxis != 0f || _verticalAxis != 0f);
    }
    
    // 플레이어 이동
    private void PlayerMove()
    {
        _moveVector = new Vector3(_horizontalAxis, 0, _verticalAxis).normalized;
        transform.position += _moveVector * _moveSpeed * Time.deltaTime;
        _playerAnimator.SetFloat("isRun", _isRun ? 1f : 0f);
    }

    // 플레이어 방향
    private void PlayerTurn()
    {
        if (_moveVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }




    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(TagType.MoneyPoint.ToString()))
        {
            if (bezierCurves != null)
            {
                npcBezierCurves.Add(bezierCurves);
            }
            foreach (Money npcBezierCurve in npcBezierCurves)
            {
                npcBezierCurve.boolThrow = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag(TagType.MoneyPoint.ToString()))
        {
            if (bezierCurves != null)
            {
                npcBezierCurves.Remove(bezierCurves);
            }
        }
    }

}
