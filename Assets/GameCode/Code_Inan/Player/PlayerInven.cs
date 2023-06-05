using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster;

namespace Player
{
    public class PlayerInven : MonoBehaviour
    {
        public static PlayerInven instance = null;
        
        int[] monsCount = new int[5];

        private int itemCnt;
        private int maxItemCnt;

        private void Awake() {
            if(instance != null)
            {
                Destroy(this.gameObject);
            }
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            itemCnt = 0;
            maxItemCnt = 5;
        }

        //그랩완료한 몬스터에 따라 아이템 량 증가
        public void AddItem(MonsterKind monKind)
        {
            if (itemCnt < maxItemCnt)
            {
                itemCnt++;
                monsCount[(int)monKind]++;

                Debug.Log($"All Item Cnt {itemCnt}");
                Debug.Log($"{System.Enum.GetName(typeof(MonsterKind), (int)monKind)} : {monsCount[(int)monKind]}");
            }
            else
            {
                Debug.Log("크기 초과");
            }
            
        }

    }

}
