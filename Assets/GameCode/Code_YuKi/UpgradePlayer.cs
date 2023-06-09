using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using UnityEngine.UI;
using Player;

public class UpgradePlayer : MonoBehaviour
{

    [SerializeField] private GameObject upgradeCanvas;
    PlayerManager pm;
    UIManager uim;

    [Header("Capacity")]
    [SerializeField] private Button capacityUpgradeBtn;
    [SerializeField] private Text capacityUpgradeCostText;
    [SerializeField] private Text capacityLevelText;
    int capacityLevel = 1;
    int capacityCost = 100;
    PlayerInvenMaster pim;


    [Header("Speed")]
    [SerializeField] private Button speedUpgradeBtn;
    [SerializeField] private Text speedUpgradeCostText; 
    [SerializeField] private Text speedLevelText;
    int speedLevel = 1;
    int speedCost = 400;

    PlayerControllerMaster pcm;


    private void Start() 
    {

        upgradeCanvas.SetActive(false);
        
        pm = PlayerManager.instance;
        pcm = PlayerControllerMaster.instance;    
        pim = PlayerInvenMaster.instance;     
        uim = UIManager.instance;

    }

    private void OnTriggerEnter(Collider other) 
    {

        if (other.gameObject.CompareTag(TagType.Player.ToString()))
        {
            upgradeCanvas.SetActive(true);
        }
    }


    private void OnTriggerExit(Collider other) 
    {

        if (other.gameObject.CompareTag(TagType.Player.ToString()))
        {
            upgradeCanvas.SetActive(false);
        }
    }
    

    public void UpgradeSpeed()
    {

        // 스피드 업 & 돈 계산
        pcm._moveSpeed += 2;
        pm.money -= speedCost;


        // 다음 레벨 
        speedLevel++;
        speedLevelText.text = $"Lvl. {speedLevel}";

        speedCost += 200;
        speedUpgradeCostText.text = speedCost.ToString();


    }

    public void UpgradeCapacity()
    {

        // 용량 업 & 돈 계산
        pim.maxItemCnt += 2;
        pm.money -= capacityCost;

        // UI
        uim.SetMosterballUI(pim.itemCnt, pim.maxItemCnt);

        // 다음 레벨 
        capacityLevel++;
        capacityLevelText.text = $"Lvl. {capacityLevel}";

        capacityCost += 200;
        capacityUpgradeCostText.text = capacityCost.ToString();



    }

}
