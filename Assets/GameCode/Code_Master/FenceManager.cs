using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster;
using EnumTypes;

public class FenceManager : MonoBehaviour
{

    public MonsterKind monsterKind;
    public FenceStateType fenceState = FenceStateType.Disable;

    GameManager gm;

    [Header("Price")]
    [SerializeField] public int price = 100; // 울타리 활성화 가격
    public int priceInterval = 10;
    [SerializeField] Text priceText; // 돈 텍스트 


    
    [Header("Not Open")]
    [SerializeField] private GameObject notOpenObject; // 열리지 않았을 때 펜스
    [SerializeField] Image fenceImg; // 울타리 이미지 
    [SerializeField] Image moneyChatBubbleImg; // 돈 말풍선 이미지 
    private Vector3 closeScale = new Vector3(1f, 1f, 1f); // 이미지 축소 스케일
    private Vector3 openScale = new Vector3(1.5f, 1.5f, 1f); // 이미지 확대 스케일


    [Header("Open")]
    [SerializeField] private GameObject openObject; // 열렸을 때 펜스
    [SerializeField] private Animator anim; // 애니메이션


    [Header("Expand")]
    [SerializeField] bool isExpandWall = false; // 가로
    [SerializeField] Transform wallH; // 가로
    [SerializeField] Transform wallL; // 왼
    [SerializeField] Transform wallR; // 오
    [SerializeField] GameObject wallParticles;
    [SerializeField] float wallMoveDistance = 16f;
    [SerializeField] float wallMoveDuration = 1.5f;
    [SerializeField] GameObject nextFence;

    private void Awake() 
    {
        if (fenceState == FenceStateType.Disable)
            gameObject.SetActive(false);
    }


    private void OnEnable() 
    {
        fenceState = FenceStateType.NotOpen;    
    }

    void Start()
    {        
        gm = GameManager.instance;
        priceText.text = price.ToString();    

    }

    // Update is called once per frame
    void Update()
    {

        // switch(fenceState)
        // {                
        // }
        
    }


    private void OnTriggerEnter(Collider other) 
    {
        GameObject triggerObject = other.gameObject;

        if (triggerObject.CompareTag(TagType.Player.ToString()))
        {
            // 열리지 않았다면
            if (fenceState == FenceStateType.NotOpen)
            {   
                NotOpenTrigger(true);
            }   
        }
    }

    private void OnTriggerStay(Collider other) 
    {

        GameObject triggerObject = other.gameObject;

        if (triggerObject.CompareTag(TagType.Player.ToString()))
        {
            // 열리지 않았다면
            if (fenceState == FenceStateType.NotOpen)
            {   
                priceText.text = price.ToString(); 
                if (price == 0) // 가격 다 내서 오픈
                {
                    fenceState = FenceStateType.Open;
                    StartCoroutine(Activate());
                }
            }   
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        GameObject triggerObject = other.gameObject;

        if (triggerObject.CompareTag(TagType.Player.ToString()))
        {
            // 열리지 않았다면
            if (fenceState == FenceStateType.NotOpen)
            {   
                NotOpenTrigger(false);
            }   
        }
    }

    private void NotOpenTrigger(bool isEnter)
    {
        if (isEnter)
        {
            // 이미지 확대 
            StartCoroutine(ChangeImgScale(fenceImg.transform, openScale, 0.5f));
            StartCoroutine(ChangeImgScale(moneyChatBubbleImg.transform, openScale, 0.5f));
        }
        else
        {
            // 이미지 축소 
            StartCoroutine(ChangeImgScale(fenceImg.transform, closeScale, 0.5f));
            StartCoroutine(ChangeImgScale(moneyChatBubbleImg.transform, closeScale, 0.5f));
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

        notOpenObject.SetActive(false);
        openObject.SetActive(true); 

        anim.SetTrigger(AnimType.activate.ToString());
        

        // 확장 땅이 아닐 경우 레벨업
        if (!isExpandWall)
        {
            gm.PlayerLevelUp();
        }
            
        else
        {
            yield return MoveWalls();
         }

        gm.OpenFence();

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
    }
}
