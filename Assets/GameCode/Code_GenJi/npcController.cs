using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcController : MonoBehaviour
{
    private LineArea _lineArea; // NPC가 속한 LineArea

    public void MoveToLineArea(LineArea lineArea)
    {
        if (_lineArea != null)
        {
            // 이전 LineArea에서 제거
            // _lineArea.RemoveNPC(gameObject);

            Debug.Log("fffff");
        }

            Debug.Log("ddd");
        // 새로운 LineArea로 이동
        _lineArea = lineArea;
        // _lineArea.AddNPC(gameObject);
    }
}
