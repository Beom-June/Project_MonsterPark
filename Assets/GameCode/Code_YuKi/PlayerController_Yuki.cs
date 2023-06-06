using System;
using System.Collections;
using System.Collections.Generic;
using BrokenVector.LowPolyFencePack;
using UnityEngine;
using EnumTypes;

public class PlayerController_Yuki : MonoBehaviour
{

    public PlayerStateType playerState = PlayerStateType.None;
    public Money money;

    #region instance
    public static PlayerController_Yuki Instance;
    private static PlayerController_Yuki instance
    {
        get
        {
            if (Instance != null) return Instance;
            if (PlayerController_Yuki.instance == null)
            {
                Debug.LogError("PlayerController_Yuki is null!");
            }
            Instance = PlayerController_Yuki.instance;
            return Instance;
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    
    #endregion

    private void Start()
    {
        money = GetComponent<Money>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject triggerObject = other.gameObject;
        
        
        // 카운터 계산 시작
        if (triggerObject.CompareTag(TagType.Counter.ToString()))
        {
            playerState = PlayerStateType.Sell;
        }
        // // 울타리
        // else if (triggerObject.CompareTag(TagType.Fence.ToString()))
        // {
        //     playerState = PlayerStateType.Buy;
        // }
    }
    
    private void OnTriggerExit(Collider other)
    {
        GameObject triggerObject = other.gameObject;
        
        
        // 카운터 계산 끝
        if (triggerObject.CompareTag(TagType.Counter.ToString()))
        {
            playerState = PlayerStateType.None;
        }
        // // 울타리 계산
        // else if (triggerObject.CompareTag(TagType.Fence.ToString()))
        // {
        //     playerState = PlayerStateType.None;
        // }
    }

}
