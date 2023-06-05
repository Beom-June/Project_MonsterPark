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
        [SerializeField] private Image img;
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

        Sensor sensor;

        private bool isGrabbed = false;
        private bool isPlayerChase = false;
        private void Awake()
        {
            //img = GetComponentInChildren<Image>();
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
                img.fillAmount -= Time.deltaTime;
                closestRenderer.material.color = Color.red;
            }
            else if(monsterState == MonsterState.IDLE)
            {
                closestRenderer.material.color = closestOriginalColor;
                img.fillAmount = 1.0f;
            }
            else if(monsterState == MonsterState.GRABBED)
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
                if (img.fillAmount <= 0.0f)
                {
                    monsterState = MonsterState.GRABBED;
                }
                else if (isPlayerChase)
                {
                    monsterState = MonsterState.ESCAPE;
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
                        Debug.Log("IDLE");
                        break;
                    case MonsterState.ESCAPE:
                        Debug.Log("escape");
                        break;
                    case MonsterState.GRABBED:
                        isGrabbed = true;
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

        public void SetMonImage()
        {
            img.fillAmount -= Time.deltaTime;
        }
        



    }
}

