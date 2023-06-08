using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

using UnityEngine.AI;
namespace Monster
{
    public class MonsterController : MonoBehaviour
    {
        PlayerInven playerInven;
        PlayerController_Inan playerCtr; // 코드 충돌 조심
        [SerializeField] private Image hpBarImg; // 체력 바
        [SerializeField] private GameObject hpObj; //비활성화 시킬 체력 Obj
        [SerializeField] private MonsterState monsterState = MonsterState.IDLE;
        [SerializeField] private MonsterKind monsterKind = MonsterKind.NONE;

        private Color closestOriginalColor;
        [SerializeField] private SkinnedMeshRenderer closestRenderer;
        private Vector3 currentScale;
        private Vector3 currentPosition;
//-------------IDLE Movement
        [Header("IDLE Movement")]
        private readonly int hashWalk = Animator.StringToHash("IsWalk");
        //private readonly int hashEscape = Animator.StringToHash("IsEscape"); // 센서에 발각시 한번만 놀라서 실행할 거임.
        [SerializeField] float movementSpeed = 3f;  // 몬스터의 이동 속도
        [SerializeField] private float idleMoveTime = 10.0f;

        private Animator anim;
        private NavMeshAgent agent;
        private Vector3 targetPosition;  // 이동할 목표 위치

        private Vector3 runPosition;  // 이동할 목표 위치

        private float idleTime = 0f;
        private float time = 0f;

        private float idleT = 0f;
        private float idleTimer = 2f;

//Level diffrent Event
        [SerializeField] private GameObject levelObj;
        private bool isLock;
        private int monLevel;
        private int playerLevel;
        


//-------------------Grabbed Monster
        [Header("Grabbed Monster")]
        [SerializeField] private float scaleDownTime = 0.5f; // 줄어드는 속도
        private float currentTime;
        private float t;
        [SerializeField] private float rotSeepd = 1000.0f;

        Quaternion dir;

        private bool isGrabbed = false;
        private bool isPlayerChase = false;

        
        private void Awake()
        {
            closestRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            closestOriginalColor = closestRenderer.material.color;
        }

        void Start()
        {
            playerInven = PlayerInven.instance;
            playerCtr = PlayerController_Inan.instance; // 코드 충돌 조심
            currentScale = this.transform.localScale;
            
            switch(monsterKind)
            {
                case MonsterKind.ONE:
                    monLevel = 1;
                    break;
                case MonsterKind.TWO:
                    monLevel = 2;
                    break;
                case MonsterKind.THREE:
                    monLevel = 3;
                    break;
                case MonsterKind.FOUR:
                    monLevel = 4;
                    break;
                case MonsterKind.FIVE:
                    monLevel = 5;
                    break;
            }

            SetRandomTargetPosition();
            agent.SetDestination(targetPosition);

            StartCoroutine(CheckMonsterState());
            StartCoroutine(MonsterAction());
        }

        // Update is called once per frame
        void Update()
        {

            if (monsterState == MonsterState.IDLE)
            {
                idleT += Time.deltaTime;
                if(idleT >= idleTimer)
                {
                    monsterState = MonsterState.WALK;
                    idleT = 0.0f;
                }
            }
            else
            {
                idleT = 0.0f;
            }


            if(monsterState == MonsterState.ESCAPE)
            {
                hpBarImg.fillAmount -= Time.deltaTime * 0.5f;
                closestRenderer.material.color = Color.red;
            }

            if(monsterState == MonsterState.GRABBED)
            {
                if(t <= 1.0f)
                {
                    currentTime += Time.deltaTime;
                    t = currentTime / scaleDownTime;
                    transform.localScale = Vector3.Lerp(currentScale, Vector3.zero, t); 
                    
                }
                transform.Rotate(Vector3.up * Time.deltaTime * rotSeepd);
                transform.position = Vector3.Lerp(currentPosition, playerCtr.GetMonsterBallPos().position, t);
            }
        }

        IEnumerator CheckMonsterState()
        {
            while (!isGrabbed)
            {
                if (isPlayerChase && !isLock)
                {
                    monsterState = MonsterState.ESCAPE;
                    if (hpBarImg.fillAmount <= 0.0f)
                    {
                        monsterState = MonsterState.GRABBED;
                    }
                }
            
                if(!isPlayerChase && monsterState == MonsterState.ESCAPE)
                {
                    monsterState = MonsterState.IDLE;
                }

                yield return new WaitForSeconds(0.02f);

            }
        }

        IEnumerator MonsterAction()
        {
            while(!isGrabbed)
            {
                switch(monsterState)
                {
                    case MonsterState.IDLE:
                        agent.isStopped = true;
                        anim.SetBool(hashWalk, false);

                        closestRenderer.material.color = closestOriginalColor;
                        hpObj.SetActive(false);
                        hpBarImg.fillAmount = 1.0f;
                        break;

                    case MonsterState.WALK:

                        agent.isStopped = false;
                        agent.speed = 3f;
                        anim.SetBool(hashWalk, true);

                        agent.SetDestination(targetPosition);
                        if (agent.velocity == Vector3.zero)
                        {
                            SetRandomTargetPosition();
                        }

                        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                        {
                            SetRandomTargetPosition();
                            monsterState = MonsterState.IDLE;   
                        }
                        break;

                    case MonsterState.ESCAPE:
                        agent.isStopped = false;
                        agent.speed = 5f;
                        
                        anim.SetBool(hashWalk, true);
                       
                        EscapeToPlayer();
                        agent.SetDestination(runPosition);
                     
                        currentPosition = this.transform.position;
                        hpObj.SetActive(true);
                        break;

                    case MonsterState.GRABBED:
                        isGrabbed = true;
                        hpObj.SetActive(false);
                        
                        
                        Debug.Log("grabbed");
                        yield return new WaitForSeconds(1.0f);
                        Destroy(gameObject);
                        playerInven.AddItem(this.monsterKind);
                        break;
                }
                yield return new WaitForSeconds(0.02f);
            }
        }


        private void SetRandomTargetPosition()
        {
            // 무작위로 새로운 목표 위치 설정 (예시로 -10에서 10 사이의 범위로 설정)
            float randomX = Random.Range(transform.position.x - 10f, transform.position.x + 10f);
            float randomZ = Random.Range(transform.position.z -10f, transform.position.z + 10f);
            targetPosition = new Vector3(randomX, transform.position.y, randomZ);
      
            idleTimer = Random.Range(0.3f , 3.0f);
        }

        private void EscapeToPlayer()
        {
            
            Transform playerTr = playerCtr.transform;
           

            // 무작위로 새로운 목표 위치 설정 (예시로 -10에서 10 사이의 범위로 설정)
            float randomX = Random.Range(-2f, 2f);
            float randomZ = Random.Range(-2f, 2f);
             Vector3 direction = transform.position - playerTr.position;
            runPosition = transform.position + direction.normalized * 6f;
            

            // targetPosition = new Vector3(randomX, 0, randomZ);
        }
        public void SetIsPlayerChase(bool _isPlayerChase)
        {
            this.isPlayerChase = _isPlayerChase;
        }

        public void CheckLevelLock(int _playerLevel)
        {
            if(monLevel > _playerLevel)
            {   
                isLock = true;
                levelObj.SetActive(true);
            }
        }

        public void SetLevelLock()
        {
            isLock = false;
            levelObj.SetActive(false);
        }

        public bool GetLevelLock()
        {
            return isLock;
        }

        public bool GetIsPlayerChase()
        {

            return this.isPlayerChase;
        }

        public bool GetMonsterIsGrabbed()
        {
            return isGrabbed;
        }

        public void SetMonImage()
        {
            hpBarImg.fillAmount -= Time.deltaTime;
        }
    }
}

