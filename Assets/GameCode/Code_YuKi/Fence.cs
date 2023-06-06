using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
public class Fence : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        GameObject triggerObject = other.gameObject;
        
        
        // 카운터 계산 시작
        if (triggerObject.CompareTag(TagType.Player.ToString()))
        {
            PlayerController_Yuki.Instance.money.SetEndPoint(transform);
            PlayerController_Yuki.Instance.money.ThrowMoney();
        }
    }
}