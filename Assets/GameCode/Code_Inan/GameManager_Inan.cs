using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster;
using Player;

public class GameManager_Inan : MonoBehaviour
{
    public static GameManager_Inan instance = null;

    PlayerSensor playerSensorCtr;    

    [Header("Monster UI")] //우측 상단 UI
    [SerializeField] private Sprite[] monsTexture = new Sprite[5];
    private GameObject[] monsObj = new GameObject[5];
    private Text[] monsText = new Text[5];
    private bool[] checkTexture = new bool[5];
    [SerializeField] private GameObject panelObj;
    [SerializeField] private GameObject monPrefab;

    [Header("Player UI")] //좌측 상단 UI / PlayerUI
    [SerializeField] private Text monsBallCntText;
    [SerializeField] private GameObject maxUI;
    
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
        playerSensorCtr = PlayerSensor.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
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
            playerSensorCtr.SetIsMaxItem(false);
            monsBallCntText.text = $"{curItemCnt}/{maxItemCnt}";
            maxUI.SetActive(false);
        }
       

    }
}
