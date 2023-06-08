using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance = null;

    public delegate void PayMoney(int money);
    public PayMoney payMoney;

    [Header("Level")]
    [SerializeField] private int level = 0;

    #region Property
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    #endregion


    [Header("Money")]
    public int money = 100;
    [SerializeField] MoneyPile stackMoney;
    private bool isGetMoney = false;
    private FenceManager fm; // 현재 닿아있는 펜스 

    private Money moneyController; // 플레이어의 돈 
    public bool isInCounter = false; // 카운터에 있는지

    
    [Header("Monster")]
    public int maxItemCnt = 5; // 잡을 수 있는 몬스터 수
    

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
        moneyController = GetComponent<Money>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        GameObject triggerObject = other.gameObject;

        if (triggerObject.CompareTag(TagType.NotOpenFence.ToString()))
        {
            // 현재 닿아있는 펜스 설정
            fm = triggerObject.GetComponent<FenceManager>();
        }
        else if (triggerObject.CompareTag(TagType.MoneyPoint.ToString()))
        {
            isInCounter = true;
        }
    }
    

    private void OnTriggerStay(Collider other) 
    {
        GameObject triggerObject = other.gameObject;

        if (triggerObject.CompareTag(TagType.NotOpenFence.ToString()))
        {

            if (fm != null)
            {
                // 열리지 않았다면
                if (fm.fenceState == FenceStateType.NotOpen)
                {   
                    // 구입할 수 있을 떄
                    if (fm.price > 0 && (money-fm.priceInterval) >= 0)
                    {
                        // 돈 던지기
                        moneyController.SetEndPoint(triggerObject.transform);
                        moneyController.ThrowMoney();

                        // 계산
                        money -= fm.priceInterval;
                        // 10, 20, 30, 40 .. 원씩 가져가게 고치기 
                        // 우선은 10원씩
                        fm.price -= fm.priceInterval;
                        // fm.priceInterval += 10;
                    }     
                    else 
                    {
                        // 가격을 모두 지불했거나
                        // 플레이어 돈이 모자라거나..
                    }              
                }
            }
               
        }

    }

    private void OnTriggerExit(Collider other) 
    {
         GameObject triggerObject = other.gameObject;

         if (triggerObject.CompareTag(TagType.MoneyPoint.ToString()))
        {
            isInCounter = false;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.collider.gameObject.CompareTag(TagType.Money.ToString()) && !isGetMoney)
        {
            Debug.Log("Get Money");
            StartCoroutine(stackMoney.RemoveObjects());
            isGetMoney = true;
        }    
    }
}
