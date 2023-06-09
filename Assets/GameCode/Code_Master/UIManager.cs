using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

public class UIManager : MonoBehaviour
{

    public static UIManager instance = null;

    PlayerSensorMaster playerSensorCtr;    

    [Header("Monster UI")] //���� ��� UI
    [SerializeField] private Sprite[] monsTexture = new Sprite[5];
    private GameObject[] monsObj = new GameObject[5];
    private Text[] monsText = new Text[5];
    private bool[] checkTexture = new bool[5];
    [SerializeField] private GameObject panelObj;
    [SerializeField] private GameObject monPrefab;

    [Header("Player UI")] //���� ��� UI / PlayerUI
    [SerializeField] private Text monsBallCntText;
    [SerializeField] private GameObject maxUI;

    [SerializeField] private GameObject plusMonsterUI;  // 여기 추가
    [SerializeField] private Image plusMonsterImg;  // 여기 추가
    // yuki 추가

    PlayerManager pm;
    GameManager gm;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text levelText;

    [SerializeField] private GameObject levelUpCanvas;
    [SerializeField] private Image monsterImg;
    [SerializeField] private Text monsterName;

    string[] monsterNames = { "EGG", "WOLF", "SNAKE", "BABY DRAGON", "DRAGON"};

    
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }    
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        playerSensorCtr = PlayerSensorMaster.instance;
        pm = PlayerManager.instance;
        gm = GameManager.instance;

        gm.levelUp += ChangeLevel;

    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = pm.money.ToString();
        levelText.text = pm.Level.ToString();
    }

    void ChangeLevel(int level)
    {
        StartCoroutine(ActiveLevelUpCanvas(level));  
    }

    IEnumerator ActiveLevelUpCanvas(int level)
    {
        yield return new WaitForSeconds(1f);

        monsterImg.sprite = monsTexture[level-1];
        monsterName.text = monsterNames[level-1];
        levelUpCanvas.SetActive(true);
    }

    public void AddMonsterUI(int monidx, int monsCount, int curItemCnt, int maxItemCnt)
    {
        if (!checkTexture[monidx])
        {
            checkTexture[monidx] = true;
            monsObj[monidx] = Instantiate(monPrefab, panelObj.transform.position, Quaternion.identity);

            Image monImg = monsObj[monidx].transform.GetChild(0).GetComponent<Image>();
            monsText[monidx] = monsObj[monidx].GetComponentInChildren<Text>();
            monImg.sprite = monsTexture[monidx];
            monsObj[monidx].transform.parent = panelObj.transform;

            plusMonsterImg.sprite = monsTexture[monidx];
        }
        else
        {
            monsText[monidx].text = $"{monsCount}";
        }

        if(curItemCnt / maxItemCnt == 1)
        {
            playerSensorCtr.SetIsMaxItem(true);
            monsBallCntText.text = $"Max!";
            maxUI.SetActive(true);
        }
        else
        {
            // StartCoroutine(FloatingPlusMonsterUI()); // 여기 추가
            playerSensorCtr.SetIsMaxItem(false);
            monsBallCntText.text = $"{curItemCnt}/{maxItemCnt}";
            maxUI.SetActive(false);
        }
    }

    public void SetMosterballUI(int curItemCnt, int maxItemCnt)
    {
        Debug.Log($"{curItemCnt} {maxItemCnt}");
        monsBallCntText.text = $"{curItemCnt}/{maxItemCnt}";
    }

    public IEnumerator FloatingPlusMonsterUI()
    {
        plusMonsterUI.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        plusMonsterUI.SetActive(false);
    }
}
