using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster;

namespace Player
{
    public class PlayerInvenMaster : MonoBehaviour
    {
        public static PlayerInvenMaster instance = null;
        UIManager gmr;
        int[] monsCount = new int[5];
        [SerializeField] private GameObject[] monsObj = new GameObject[5];

        [SerializeField] private Transform monBallPos;
        [SerializeField] private GameObject monBallPrefab;

        public int itemCnt;
        public int maxItemCnt;
        
        private float time;
        private bool isCageOut = false;
   

        private void Awake() {
            if(instance != null)
            {
                Destroy(this.gameObject);
            }
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            gmr = UIManager.instance;
            itemCnt = 0;
            maxItemCnt = 5;
        }

        //그랩완료한 몬스터에 따라 아이템 량 증가
        public void AddItem(MonsterKind monKind)
        {
            if (itemCnt < maxItemCnt)
            {
                itemCnt++;
                monsCount[(int)monKind]++;
                
                gmr.AddMonsterUI((int)monKind, monsCount[(int)monKind], itemCnt, maxItemCnt);
                //Debug.Log($"All Item Cnt {itemCnt}");
                //Debug.Log($"{System.Enum.GetName(typeof(MonsterKind), (int)monKind)} : {monsCount[(int)monKind]}");
            }
            else
            {
                Debug.Log("크기 초과");
            }
            
        }

        private void OnTriggerEnter(Collider coll)
        {   

            if(coll.CompareTag("Fence"))
            {
                isCageOut = false;
                FenceController fenceCtr = coll.GetComponent<FenceController>();
                Debug.Log("팬스 충돌");
                switch(fenceCtr.GetMonsState())
                {
                    case MonsterKind.ONE:
                        if (monsCount[(int)MonsterKind.ONE] != 0)
                        {
                            StartCoroutine(CreateMonBall((int)MonsterKind.ONE, fenceCtr));
                        }

                        break;

                    case MonsterKind.TWO:
                        if(monsCount[(int)MonsterKind.TWO] != 0)
                        {
                            StartCoroutine(CreateMonBall((int)MonsterKind.TWO, fenceCtr));
                        }
                        break;

                    case MonsterKind.THREE:
                        if(monsCount[(int)MonsterKind.THREE] != 0)
                        {
                            StartCoroutine(CreateMonBall((int)MonsterKind.THREE, fenceCtr));
                        }
                        break;

                    case MonsterKind.FOUR:
                        if(monsCount[(int)MonsterKind.FOUR] != 0)
                        {
                            StartCoroutine(CreateMonBall((int)MonsterKind.FOUR, fenceCtr));
                        }
                        break;
                }

            }
        }

        private void OnTriggerExit(Collider coll)
        {
            if(coll.CompareTag("Fence"))
            {
                Debug.Log("Fence Out");
                isCageOut = true;
            }
            
        }

        IEnumerator CreateMonBall(int monIdx, FenceController _fenceCtr)
        {
            while (monsCount[monIdx] != 0 && !isCageOut)
            {
                GameObject monBallObj = Instantiate(monBallPrefab, monBallPos.position, Quaternion.identity);
                Transform targetTr = _fenceCtr.GetRandDest();
                //_fenceCtr.AddMonCount();
                MonsterballControllerMaster monBallCtr = monBallObj.GetComponent<MonsterballControllerMaster>();
                monBallCtr.MonsterBall_Init(monBallPos, targetTr, true, monsObj[monIdx], 5.0f, _fenceCtr);
              
                itemCnt--;
                monsCount[monIdx]--;
                gmr.AddMonsterUI(monIdx, monsCount[monIdx], itemCnt, maxItemCnt);

                yield return new WaitForSeconds(1.0f);
                Destroy(monBallObj);
            }
        }

        public int GetMonsCount(MonsterKind monKind)
        {
            if(monsCount[(int)monKind] != 0)
            {
                return monsCount[(int)monKind];
            }
            else
            {
                return 0;
            }
        }

    }

}
