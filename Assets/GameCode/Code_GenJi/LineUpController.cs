using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpController : MonoBehaviour
{
    [SerializeField] private List<Transform> _lineArea;                     //  NPC가 줄 서는 위치
    [SerializeField] private List<bool> _lineAreaHasChild;                  //  LineArea의 자식 여부를 나타내는 리스트
    private bool _reachedFirstPoint = false; // 첫 번째 포인트 도착 여부를 나타내는 변수
    [SerializeField] private List<SafariNpcController> _safariNpcList = new List<SafariNpcController>();
    SafariNpcController _safariNpc;

    void Start()
    {
        // _safariNpc = GetComponent<SafariNpcController>();
        // LineUpNPCs();
        InitializeLineAreaHasChild();
                // 모든 NPC 찾기
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npcObject in npcObjects)
        {
            _safariNpc = npcObject.GetComponent<SafariNpcController>();
            if (_safariNpc != null)
            {
                _safariNpcList.Add(_safariNpc);
            }
        }
    }

    void Update()
    {
        CheckLineAreaChildren();
    }

    // private void LineUpNPC()
    // {
    //     // 각 하위 트랜스폼에 순서대로 줄섬
    //     for (int i = 0; i < _lineArea.Count; i++)
    //         if (_lineArea[i] != null)
    //         {
    //             {
    //                 // NPC를 _lineArea 리스트에 있는 위치로 이동시킵니다.
    //                 _lineArea[i].position = transform.position + new Vector3(i * 2, 0, 0);

    //                 // NPC가 LineUpController를 부모로 설정합니다.
    //                 _lineArea[i].parent = transform;
    //             }
    //         }
    // }

    public void CheckLineAreaChildren()
    {
        for (int i = 0; i < _lineArea.Count; i++)
        {
            LineArea lineArea = _lineArea[i].GetComponent<LineArea>();
            if (lineArea.HasChildren())
            {
                Debug.Log(_lineArea[i].name);
                _lineAreaHasChild[i] = true;
                _safariNpc.WaitAtFirstWaypoint(10f);
                if (i == 0)
                {
                }
                if (i > 0)
                {
                    Transform previousLineArea = _lineArea[i - 1];
                    Transform currentLineArea = _lineArea[i];

                    if (previousLineArea.childCount > 0)
                    {
                        Transform childNPC = previousLineArea.GetChild(0);

                        if (!_lineAreaHasChild[i - 1])
                        {
                            // 첫 번째 LineArea의 자식이 없어지면 두 번째 LineArea의 자식을 첫 번째 LineArea의 자식으로 이동
                            childNPC.position = currentLineArea.position;
                            childNPC.parent = currentLineArea;
                        }
                        else
                        {
                            // 첫 번째 LineArea의 자식이 아직 있을 경우 두 번째 자식으로 보내지 않고 다시 첫 번째 LineArea로 돌려보냄
                            childNPC.position = previousLineArea.position;
                            childNPC.parent = previousLineArea;
                        }
                    }
                }
            }
            else
            {
                _lineAreaHasChild[i] = false;
            }
        }
    }

    public Transform FindAvailableLineArea()
    {
        for (int i = 0; i < _lineArea.Count; i++)
        {
            if (_lineArea[i] != transform && !_lineAreaHasChild[i])
            {
                return _lineArea[i];
            }
        }
        return null;
    }


    // _lineArea의 추가 갯수 만큼 _lineAreaHasChild의 bool 값의 갯수를 자동으로 추가해줌
    private void InitializeLineAreaHasChild()
    {
        _lineAreaHasChild = new List<bool>(_lineArea.Count);
        for (int i = 0; i < _lineArea.Count; i++)
        {
            _lineAreaHasChild.Add(false);
        }
    }

    private void AddLineArea()
    {
        // LineArea 오브젝트 추가할 때
        _lineAreaHasChild.Add(false);
    }

    private void RemoveLineArea(int index)
    {
        // index 위치의 LineArea 오브젝트 제거할 때
        _lineArea.RemoveAt(index);
        _lineAreaHasChild.RemoveAt(index);
    }
}
