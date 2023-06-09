using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster;

namespace Player
{
    public class PlayerControllerMaster : MonoBehaviour
    {
        [Header("Player Settings")]
        public float _moveSpeed = 5f;                //   이동 속도
        [SerializeField] private float _rotationSpeed = 5f;            //   이동 속도
        private float _horizontalAxis;                                  //  수평 입력 값
        private float _verticalAxis;                                    //  수직 입력 값
        private bool _isRun;                                            //  달리기 bool 값
        private Vector3 _moveVector;                                    //  moveVector 저장 
        private Animator _playerAnimator;                               //  Animator 저장
        private Rigidbody _playerRigidbody;                             //  Rigidbody 저장
                                                                        ///// Add Info
        public static PlayerControllerMaster instance = null;

        [SerializeField] private Transform monsterBallPos;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            _playerAnimator = GetComponent<Animator>();
            _playerRigidbody = GetComponent<Rigidbody>();
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

        public Transform GetMonsterBallPos()
        {
            return monsterBallPos;
        }

    
 

    }

}
