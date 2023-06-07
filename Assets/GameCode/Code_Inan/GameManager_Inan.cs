using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster;
public class GameManager_Inan : MonoBehaviour
{
    public static GameManager_Inan instance = null;


    [SerializeField] private Sprite[] monsTexture = new Sprite[5];
    private GameObject[] monsObj = new GameObject[5];
    private Text[] monsText = new Text[5];
    private bool[] checkTexture = new bool[5];
    [SerializeField] private GameObject panelObj;
    [SerializeField] private GameObject monPrefab;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMonsterUI(int monidx, int monsCount)
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

    }
}
