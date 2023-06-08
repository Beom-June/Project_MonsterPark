using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpController : MonoBehaviour
{
    [SerializeField] private List<Transform> _lineArea;                     //  NPC가 줄 서는 위치
    [SerializeField] private List<bool> _lineAreaHasChild;                  //  LineArea의 자식 여부를 나타내는 리스트
    private bool _reachedFirstPoint = false; // 첫 번째 포인트 도착 여부를 나타내는 변수

    void Start()
    {
        // LineUpNPCs();
        InitializeLineAreaHasChild();
    }

    void Update()
    {
        CheckLineAreaChildren();
    }

    private void LineUpNPC()
    {
        // 각 하위 트랜스폼에 순서대로 줄섬
        for (int i = 0; i < _lineArea.Count; i++)
            if (_lineArea[i] != null)
            {
                {
                    // NPC를 _lineArea 리스트에 있는 위치로 이동시킵니다.
                    _lineArea[i].position = transform.position + new Vector3(i * 2, 0, 0);

                    // NPC가 LineUpController를 부모로 설정합니다.
                    _lineArea[i].parent = transform;
                }
            }
    }

    // public void CheckLineAreaChildren()
    // {
    //     for (int i = 0; i < _lineArea.Count; i++)
    //     {
    //         LineArea lineArea = _lineArea[i].GetComponent<LineArea>();
    //         if (lineArea.HasChildren())
    //         {
    //             // LineArea에 자식이 추가되었음을 감지
    //             // Debug.Log("LineArea에 자식이 추가");
    //             Debug.Log(_lineArea[i].name);

    //             _lineAreaHasChild[i] = true;

    //             // 이전 LineArea에서 NPC를 가져와서 현재 LineArea로 이동시킴
    //             if (i > 0)
    //             {
    //                 Transform previousLineArea = _lineArea[i];
    //                 if (previousLineArea.childCount > 0)
    //                 {
    //                     Transform childNPC = previousLineArea.GetChild(0);
    //                     childNPC.position = _lineArea[i].position;
    //                     childNPC.parent = _lineArea[i];

    //                     // 첫 번째 LineArea의 자식이 없어지면 두 번째 LineArea의 자식을 첫 번째 LineArea의 자식으로 이동하지 않도록 처리
    //                     if (i == 1 && !_lineAreaHasChild[0])
    //                     {
    //                         Debug.Log("첫번째 자식 체크");
    //                         // 옮겨주고
    //                         childNPC.parent = _lineArea[i - 1];
    //                         // 포지션값 변경
    //                         childNPC.position = _lineArea[i - 1].position;
    //                     }
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             _lineAreaHasChild[i] = false;
    //         }
    //     }
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
