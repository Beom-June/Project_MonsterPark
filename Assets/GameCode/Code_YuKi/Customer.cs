using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
public class Customer : MonoBehaviour
{

    private PlayerStateType playerState = PlayerStateType.None;
    private Money money;
    private void Start()
    {
        money = GetComponent<Money>();
    }

    void Update()
    {
        
        // 플레이어 상태 업데이트
        playerState = PlayerController_Yuki.Instance.playerState;

        switch (playerState)
        {
            case PlayerStateType.Sell:
                money.ThrowMoney();
                break;
        }
    }

}
