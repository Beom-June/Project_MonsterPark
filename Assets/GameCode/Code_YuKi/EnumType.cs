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
    
}
