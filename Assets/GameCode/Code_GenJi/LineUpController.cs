using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpController : MonoBehaviour
{
    [SerializeField] private List<Transform> _lineArea;                     //  NPC가 줄 서는 위치
    [SerializeField] private List<bool> _lineAreaHasChild;                  //  LineArea의 자식 여부를 나타내는 리스트
    void Start()
    {
        // LineUpNPCs();
        InitializeLineAreaHasChild();
    }

    void Update()
    {

        // 플레이어가 특정 위치에 닿으면, NPC는 돈을 던지고 밖으로 향함
        // 플레이어와 충돌 감지 로직 등을 구현해야 합니다.


        // LineArea에 자식이 추가되었는지 체크
        // bool hasChildren = CheckLineAreaChildren();
        // if (hasChildren)
        // {
        //     Debug.Log("ffff");
        //     // LineArea에 자식이 추가되었음을 처리하는 로직을 추가합니다.
        // }
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
    public void CheckLineAreaChildren()
    {
        for (int i = 0; i < _lineArea.Count; i++)
        {
            LineArea lineArea = _lineArea[i].GetComponent<LineArea>();
            if (lineArea.HasChildren())
            {
                // LineArea에 자식이 추가되었음을 감지
                Debug.Log("LineArea에 자식이 추가");
                Debug.Log(_lineArea[i].name);

                _lineAreaHasChild[i] = true;

                // 이전 LineArea에서 NPC를 가져와서 현재 LineArea로 이동시킴
            if (i > 0)
            {
                Transform previousLineArea = _lineArea[i];
                if (previousLineArea.childCount > 0)
                {
                    Transform childNPC = previousLineArea.GetChild(0);
                    childNPC.position = _lineArea[i].position;
                    childNPC.parent = _lineArea[i];

                    // 첫 번째 LineArea의 자식이 없어지면 두 번째 LineArea의 자식을 첫 번째 LineArea의 자식으로 이동하지 않도록 처리
                    if (i == 1 && !_lineAreaHasChild[0])
                    {
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
