using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

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

//-------------------Grabbed Monster
        [SerializeField] private float scaleDownTime = 0.5f; // 줄어드는 속도
        private float currentTime;
        private float t;
        [SerializeField] private float rotSeepd = 1000.0f;

        private bool isGrabbed = false;
        private bool isPlayerChase = false;
        private void Awake()
        {
            closestRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            closestOriginalColor = closestRenderer.material.color;
        }

        void Start()
        {
            playerInven = PlayerInven.instance;
            playerCtr = PlayerController_Inan.instance; // 코드 충돌 조심
            currentScale = this.transform.localScale;
            currentPosition = this.transform.position;
           

            StartCoroutine(CheckMonsterState());
            StartCoroutine(MonsterAction());
        }

        // Update is called once per frame
        void Update()
        {
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

                if (isPlayerChase)
                {        
                    monsterState = MonsterState.ESCAPE;
                    if (hpBarImg.fillAmount <= 0.0f)
                    {
                        monsterState = MonsterState.GRABBED;
                    }
                }
                else
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
                        closestRenderer.material.color = closestOriginalColor;
                        hpObj.SetActive(false);
                        hpBarImg.fillAmount = 1.0f;
                        break;
                    case MonsterState.ESCAPE:
                        hpObj.SetActive(isPlayerChase);
                        break;
                    case MonsterState.GRABBED:
                        isGrabbed = true;
                        hpObj.SetActive(false);
                        playerInven.AddItem(this.monsterKind);
                        
                        Debug.Log("grabbed");

                        Destroy(this.gameObject, 1.0f);
                        break;
                }
                yield return new WaitForSeconds(0.02f);
            }
        }

        public void SetIsPlayerChase(bool _isPlayerChase)
        {
            this.isPlayerChase = _isPlayerChase;
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

