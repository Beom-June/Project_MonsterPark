using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class Fence : MonoBehaviour
{

    [SerializeField] bool isActive = false; // 울타리 활성화 여부
    [SerializeField] int price = 2000; // 울타리 활성화 가격
    
    private int priceInterval = 10;


    [Header("UI")]
    [SerializeField] GameObject canvases; // 캔버스
    [SerializeField] GameObject newFenceCanvas; // 활성화 전 캔버스
    [SerializeField] Image fenceImg; // 울타리 이미지 
    private Vector3 closeScale = new Vector3(1f, 1f, 1f);
    private Vector3 openScale = new Vector3(1.5f, 1.5f, 1f);
    [SerializeField] Image moneyImg; // 돈 말풍선 이미지 
    [SerializeField] Text moneyText; // 돈 텍스트 

    [SerializeField] Image monsterImg; // 몬스터 말풍선 이미지



    [Header("for Active")]
    [SerializeField] GameObject smokeParticle; // 연기 파티클
    [SerializeField] GameObject fences; // 울타리 오브젝트
    [SerializeField] Animator anim; // 애니메이션

    [Header("Expand Wall")]
    [SerializeField] bool isExpandWall = false; // 가로
    [SerializeField] Transform wallH; // 가로
    [SerializeField] Transform wallL; // 왼
    [SerializeField] Transform wallR; // 오
    [SerializeField] GameObject wallParticles;
    [SerializeField] float wallMoveDistance = 16f;
    [SerializeField] float wallMoveDuration = 1.5f;
    [SerializeField] GameObject nextFence;

    [SerializeField] CameraController cameraController;
    [SerializeField] Camera cam;

    private void Start() 
    {
        moneyText.text = price.ToString();    
        cameraController = FindObjectOfType<CameraController>();

        if (!fences.activeSelf && !canvases.activeSelf)
        {
            cameraController.SetNewCamTransform(cam.transform);
            Debug.Log(gameObject.name);
            canvases.SetActive(true);
            cameraController.isMove = true;
        }
        
    }

    // [SerializeField]
    private void OnTriggerEnter(Collider other) 
    {
        GameObject triggerObject = other.gameObject;

        if (triggerObject.CompareTag(TagType.Player.ToString()))
        {
            if (isActive)
            {
                // 몬스터 던지기
            }
            else 
            {
                // 이미지 확대 
                StartCoroutine(ChangeImgScale(fenceImg.transform, openScale, 0.5f));
                StartCoroutine(ChangeImgScale(moneyImg.transform, openScale, 0.5f));
            }

        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        GameObject triggerObject = other.gameObject;
        
        
        // 카운터 계산 시작
        if (triggerObject.CompareTag(TagType.Player.ToString()))
        {

            if (price < 0)
                price = 0;
            else
            {
                // 10, 20, 30, 40 .. 원씩 가져가게 
                price -= priceInterval;
                priceInterval += 10;
            }            

            if (isActive)
            {
                // 몬스터 던지기
            }
            else 
            {
                moneyText.text = price.ToString(); 
                // 만약 금액에 맞는 돈을 다 냈으면
                if (price == 0)
                {
                    isActive = true;
                    StartCoroutine(Activate());
                }
                else if (price > 0)
                {
                    PlayerController_Yuki.Instance.money.SetEndPoint(transform);
                    PlayerController_Yuki.Instance.money.ThrowMoney();
                }
            }

        }
    }

    private void OnTriggerExit(Collider other) 
    {
        GameObject triggerObject = other.gameObject;

        if (triggerObject.CompareTag(TagType.Player.ToString()))
        {
            if (isActive)
            {
                // 몬스터 던지기
            }
            else 
            {
                // 이미지 축소 
                StartCoroutine(ChangeImgScale(fenceImg.transform, closeScale, 0.5f));
                StartCoroutine(ChangeImgScale(moneyImg.transform, closeScale, 0.5f));

            }

        }
    }


    private IEnumerator ChangeImgScale(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = target.localScale;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            target.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        // 최종 크기로 조정
        target.localScale = targetScale;
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(1f);

        // 영역표시 + 돈 말풍선 비활성화
        newFenceCanvas.SetActive(false);
        moneyImg.gameObject.SetActive(false);

        smokeParticle.SetActive(true); // 파티클


        if (isExpandWall)
        {
            yield return MoveWalls();
        }
        fences.SetActive(true); // 펜스 생성
        anim.SetTrigger(AnimType.activate.ToString());

        yield return new WaitForSeconds(0.5f);

        // 몬스터 말풍선 활성화
        if (!isExpandWall)
        {
            monsterImg.gameObject.SetActive(true);
            StartCoroutine(ChangeImgScale(monsterImg.transform, openScale, 0.5f));
        }
          
    }
    
    private IEnumerator MoveWalls()
    {
        wallH.gameObject.SetActive(true);
        wallL.gameObject.SetActive(true);
        wallR.gameObject.SetActive(true);
        wallParticles.SetActive(true);

        Vector3 wallHStartPosition = wallH.position;
        Vector3 wallLStartPosition = wallL.position;
        Vector3 wallRStartPosition = wallR.position;

        float elapsedTime = 0f;
        while (elapsedTime < wallMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / wallMoveDuration);


            // 첫 번째 벽을 아래로 이동
            Vector3 wallHTargetPosition = wallHStartPosition + Vector3.down * wallMoveDistance;
            wallH.position = Vector3.Lerp(wallHStartPosition, wallHTargetPosition, t);

            // 두 번째 벽을 왼쪽으로 이동
            Vector3 wallLTargetPosition = wallLStartPosition + Vector3.left * wallMoveDistance;
            wallL.position = Vector3.Lerp(wallLStartPosition, wallLTargetPosition, t);

            // 세 번째 벽을 오른쪽으로 이동
            Vector3 wallRTargetPosition = wallRStartPosition + Vector3.right * wallMoveDistance;
            wallR.position = Vector3.Lerp(wallRStartPosition, wallRTargetPosition, t);

            yield return null;
        }

        // 최종 위치로 조정
        wallH.position = wallHStartPosition + Vector3.down * wallMoveDistance;
        wallL.position = wallLStartPosition + Vector3.left * wallMoveDistance;
        wallR.position = wallRStartPosition + Vector3.right * wallMoveDistance;

        nextFence.SetActive(true);
    }
}