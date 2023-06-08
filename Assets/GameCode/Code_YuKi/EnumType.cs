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
        MoneyPoint,
        Fence,
    }

    public enum AnimType
    {
        activate,

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
