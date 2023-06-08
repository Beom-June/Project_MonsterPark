using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueLineUp : MonoBehaviour
{
    [SerializeField] private List<Transform> lineArea; // NPC�� �� ���� ��ġ
    private Queue<Transform> lineQueue; // �� �� �ִ� NPC���� ť

    private void Start()
    {
        lineQueue = new Queue<Transform>();

        // ��� NPC ã��
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npcObject in npcObjects)
        {
            NewNpcController safariNpc = npcObject.GetComponent<NewNpcController>();
            if (safariNpc != null)
            {
                lineQueue.Enqueue(safariNpc.transform); // NPC�� �� �� �ִ� ť�� �߰�
            }
        }

        if (lineQueue.Count > 0)
        {
            Transform firstNpc = lineQueue.Peek();
            firstNpc.GetComponent<NewNpcController>().WaitAtWaypoint(10f); // ù ��° NPC�� ��� ���·� ����
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
                    lineQueue.Dequeue(); // ���� NPC�� �ٿ��� ����
                    lineQueue.Enqueue(currentNpc); // ���� NPC�� ���� �������� �̵�
                    currentNpc.SetParent(nextLineArea); // ���� LineArea�� �ڽ����� ����
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
