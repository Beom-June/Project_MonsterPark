using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Monster;


public class CustomerController_Inan : MonoBehaviour
{
    public enum CustomerState
    {
        WALK,
        WAIT,
        PAY, // 펫을 데리고 이동 
        WAITPAY, // 계산대 앞
        OUT,
    }
    [SerializeField] private CustomerState customerState;
    [SerializeField] private MonsterKind monKind;
    
    private readonly int hashWalk = Animator.StringToHash("IsWalk");
    private readonly int hashThink = Animator.StringToHash("IsThink");

    GameManager gmr;
    [SerializeField] private Transform payPosTr; // 줄 설 곳
    [SerializeField] private Transform _outPoint; // 나가는 곳 

    [SerializeField] Transform[] fence = new Transform[4];
    [SerializeField] private FenceController fenceCtr;


    private NavMeshAgent agent;
    private Transform destTr;
    private Animator anim;
    // Start is called before the first frame update
    
    private bool isComplete; // 계산 완료 데이터
    private int trCount;

    private int monCount;


    private Money npcBezierCurve;
    private bool isPay = false; // 돈을 다 지불했다면 
    PlayerManager pm;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        gmr = GameManager.instance;
        //monKind = (MonsterKind)Random.Range(0,4);
        monKind = (MonsterKind)0;
        
        Debug.Log((int)monKind);

        trCount = fence[(int)monKind].childCount; // fence
        destTr = fence[(int)monKind].GetChild(Random.Range(0, trCount)); // Random
        
        agent.SetDestination(destTr.position);

        StartCoroutine(CheckState());
        StartCoroutine(ActionState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckState()
    {
        while(!isComplete)
        {
            yield return new WaitForSeconds(0.3f);
            //float distance = Vector3.Distance(transform.position, destTr.position);
            float distance = agent.remainingDistance;
            if(distance < 0.1f && customerState == CustomerState.WALK)
            {
                transform.LookAt(fence[(int)monKind].position);
                customerState = CustomerState.WAIT;
                yield return new WaitForSeconds(Random.Range(3.0f, 5.5f));
                int randStateNum = Random.Range(1,3);
                if(randStateNum == 1)
                {
                    SetRandomTargetPosition();
                    
                    customerState = CustomerState.WALK;
                    Debug.Log(randStateNum);
                }
                else
                {
                    Debug.Log(randStateNum);
                    CheckInCageMonster();
                    
                }
            }

            if (distance < 0.1f &&customerState == CustomerState.PAY)
            {
               customerState = CustomerState.WAITPAY;
            }

            if (customerState == CustomerState.WAITPAY)
            {
                if (pm.isInCounter && !isPay)
                {
                    npcBezierCurve.boolThrow = true;
                    isPay = true;
                    StartCoroutine(WaitAndTransition(1f, CustomerState.OUT));
                }
            }

           

        }
    }

    IEnumerator ActionState()
    {
        while(!isComplete)
        {
            yield return new WaitForSeconds(0.3f);
            switch(customerState)
            {
                case CustomerState.WALK:
                    anim.SetBool(hashThink, false);
                    anim.SetBool(hashWalk, true);

                    break;
                case CustomerState.WAIT:
                    anim.SetBool(hashWalk, false);
                    anim.SetBool(hashThink, true);
                    break;
                case CustomerState.PAY:
                    anim.SetBool(hashThink, false);
                    anim.SetBool(hashWalk, true);
                    SetDestination(payPosTr);
                    break;
                case CustomerState.OUT:
                    anim.SetBool(hashThink, false);
                    anim.SetBool(hashWalk, true);
                    SetDestination(_outPoint);
                    break;

            }
        }
    }

    private void SetRandomTargetPosition()
    {
        destTr = fence[(int)monKind].GetChild(Random.Range(0, trCount)); // Random
        agent.SetDestination(destTr.position);
    }

     private void SetDestination(Transform posTr)
    {
        destTr = posTr; // Random
        agent.SetDestination(destTr.position);
    }

    private void CheckInCageMonster()
    {
        fenceCtr = gmr.GetFencCtr((int)monKind);
        if(fenceCtr == null)
        {
            Debug.Log("팬스 컨트롤러 없음");
            return;
        }
        monCount = fenceCtr.GetMonCount();
        Debug.Log($"몬스터 개수 : {monCount}");
       
        if(monCount > 0)
        {
            customerState = CustomerState.PAY;
            fenceCtr.SubMonCount();
            GameObject monObj = fenceCtr.GetMonObj();
            MonsterControllerMaster monCtr = monObj.GetComponent<MonsterControllerMaster>();
            monCtr.SetMonsterIsGrabbed(true);
            NavMeshAgent monNav = monObj.GetComponent<NavMeshAgent>();
            monNav.enabled = false;
            
            monObj.transform.parent = this.transform; // 여기 
            monObj.transform.localPosition = -transform.right; // Vector3(3f, 0f, 0f); // 여기
           
        }
        else
        {
            Debug.Log("몬스터가 없어서 다시 웨잇");
            customerState = CustomerState.WAIT;
        }

    }

    private IEnumerator WaitAndTransition(float waitTime, CustomerState nextCustomerState)
    {
        yield return new WaitForSeconds(waitTime);
        customerState = nextCustomerState;
    }

}
