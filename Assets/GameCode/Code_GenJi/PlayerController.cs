using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float _moveSpeed = 5f;                //   이동 속도
    [SerializeField] private float _rotationSpeed = 5f;            //   이동 속도
    private float _horizontalAxis;                                  //  수평 입력 값
    private float _verticalAxis;                                    //  수직 입력 값
    private bool _isRun;                                            //  달리기 bool 값
    private Vector3 _moveVector;                                    //  moveVector 저장 
    private Animator _playerAnimator;                               //  Animator 저장
    private Rigidbody _playerRigidbody;                             //  Rigidbody 저장

    [SerializeField] private List<MoneyBezierCurves> npcBezierCurves = new List<MoneyBezierCurves>();

    private MoneyBezierCurves bezierCurves;

    // Money UI Event
    [SerializeField] private Text _moneyText;
    [SerializeField] private Transform _textRotationTransform;

    void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();

        // 모든 NPC 찾기
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npcObject in npcObjects)
        {
            bezierCurves = npcObject.GetComponent<MoneyBezierCurves>();
            if (bezierCurves != null)
            {
                npcBezierCurves.Add(bezierCurves);
            }
        }

        // text UI
        _textRotationTransform = _moneyText.transform.parent;
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

            _textRotationTransform.rotation = Quaternion.Lerp(_textRotationTransform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator RemoveMoneyCoroutine()
    {
        // moneyText.text = $"${money}";
        _moneyText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        _moneyText.gameObject.SetActive(false);

        // pm.money += money;
        // money = 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("MoneyPoint"))
        {
            //MoneyBezierCurves bezierCurves = collider.GetComponent<MoneyBezierCurves>();

            if (bezierCurves != null)
            {
                npcBezierCurves.Add(bezierCurves);
                //bezierCurves.boolThrow = true;
            }
            foreach (MoneyBezierCurves npcBezierCurve in npcBezierCurves)
            {
                npcBezierCurve.boolThrow = true;
            }
        }
        if (collider.gameObject.CompareTag("Money"))
        {
            Destroy(collider.gameObject);
            StartCoroutine(RemoveMoneyCoroutine());
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("MoneyPoint"))
        {
            //MoneyBezierCurves bezierCurves = collider.GetComponent<MoneyBezierCurves>();

            if (bezierCurves != null)
            {
                npcBezierCurves.Remove(bezierCurves);
            }
        }
    }
}