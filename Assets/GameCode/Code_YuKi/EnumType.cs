using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  EnumTypes
{
    public enum TagType
    {
        Player, 
        NPC,
        Counter,
        Money,
        MoneyPoint,
        Fence,
        NotOpenFence
    }

    public enum AnimType
    {
        activate,
        isWalk,
        isThink,
        Change,
    }

    public enum PlayerStateType
    {
        None,
        Sell,
    }

    public enum FenceStateType
    {
        Disable,
        NotOpen,
        Open,
    }

    public enum CustomerStateType
    {
        Disable,
        Entry, // 입장
        Watching, // 구경
        Standby, // 대기
        Calculation, // 계산
        Out, // 나감
    }
    
}
