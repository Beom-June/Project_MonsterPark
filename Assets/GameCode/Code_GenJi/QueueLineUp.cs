using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueLineUp : MonoBehaviour
{
    [SerializeField] private List<Transform> lineArea; // NPC가 줄 서는 위치
    private Queue<Transform> lineQueue; // 줄 서 있는 NPC들의 큐

    private void Start()
    {
        lineQueue = new Queue<Transform>();

        // 모든 NPC 찾기
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npcObject in npcObjects)
        {
            NewNpcController safariNpc = npcObject.GetComponent<NewNpcController>();
            if (safariNpc != null)
            {
                lineQueue.Enqueue(safariNpc.transform); // NPC를 줄 서 있는 큐에 추가
            }
        }

        if (lineQueue.Count > 0)
        {
            Transform firstNpc = lineQueue.Peek();
            firstNpc.GetComponent<NewNpcController>().WaitAtWaypoint(10f); // 첫 번째 NPC를 대기 상태로 설정
        }
    }

    private void Update()
    {
        if (lineQueue.Count > 0)
        {
            Transform currentNpc = lineQueue.Peek();
            LineArea lineArea = currentNpc.GetComponent<LineArea>();
            if (lineArea != null && lineArea.HasChildren())
            {
                Transform nextLineArea = FindAvailableLineArea();
                if (nextLineArea != null)
                {
                    lineQueue.Dequeue(); // 현재 NPC를 줄에서 제거
                    lineQueue.Enqueue(currentNpc); // 현재 NPC를 줄의 뒤쪽으로 이동
                    currentNpc.SetParent(nextLineArea); // 다음 LineArea의 자식으로 설정
                }
            }
        }
    }

    private Transform FindAvailableLineArea()
    {
        foreach (Transform lineAreaTransform in lineArea)
        {
            if (!lineAreaTransform.GetComponent<LineArea>().HasChildren())
            {
                return lineAreaTransform;
            }
        }
        return null;
    }
}
