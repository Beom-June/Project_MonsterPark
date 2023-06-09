using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBallController : MonoBehaviour
{
    //-----배지어
    [SerializeField] private bool isHaveMonster = false;
    [SerializeField] private bool isCatchMonster = false;
    [SerializeField] private GameObject createPariclePrefab;
    private float time = 0.0f;
    private float t = 0.0f;

   
    [SerializeField] private float bazierSpeed = 2.0f;
    private float bezierHeight;
    private bool isCreateMon = false;

    private GameObject monPrefab;
    Transform startPos;
    Transform endPos;

    private GameObject monObj; //팬스에 넘겨줄 몬스터 오브젝트
    private FenceController fenceCtr;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DampTime();
    }

    private void DampTime()
    {
        if (isHaveMonster)
        {
            if (time < bazierSpeed)
            {
                time += Time.deltaTime;
                t = time / bazierSpeed;
                transform.position = MonBezierCurves();
            }
            else
            {
                monObj = Instantiate(monPrefab, transform.position, Quaternion.identity);
                GameObject createParticle = Instantiate(createPariclePrefab, transform.position, Quaternion.identity);
                Destroy(createParticle, 0.6f);
                monObj.layer = 0;
                isHaveMonster = false;
                time = 0;
                t = 0;
                fenceCtr.AddMonCount(monObj);
                Debug.Log($"몬스터 볼에서 호출{fenceCtr.GetMonCount()}");
                
            }
        }

        if(isCatchMonster)
        {
            if (time < bazierSpeed)
            {
                time += Time.deltaTime;
                t = time / bazierSpeed;
                transform.position = MonBezierCurves();
            }
            else
            {
                GameObject createParticle = Instantiate(createPariclePrefab, transform.position, Quaternion.identity);
                createParticle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                Destroy(createParticle, 0.5f);
                isCatchMonster = false;
                time = 0;
                t = 0;
                Destroy(this.gameObject);
            }
        }
    }


    public Vector3 MonBezierCurves()
    {
        Vector3 P1 = startPos.position;
        Vector3 P2 = startPos.position + (Vector3.up * bezierHeight);
        Vector3 P3 = endPos.position + (Vector3.up * bezierHeight);
        Vector3 P4 = endPos.position;

        Vector3 A = Vector3.Lerp(P1, P2, t);
        Vector3 B = Vector3.Lerp(P2, P3, t);
        Vector3 C = Vector3.Lerp(P3, P4, t);
        Vector3 D = Vector3.Lerp(A, B, t);
        Vector3 E = Vector3.Lerp(B, C, t);
        Vector3 F = Vector3.Lerp(D, E, t);

        return F;
    }

    public void MonsterBall_Init(Transform _startPos, Transform _endPos, bool check, GameObject _monObj, float height, FenceController _fenceCtr)
    {
        if(!isHaveMonster)
        {
            startPos = _startPos;
            endPos = _endPos;
        }
        isHaveMonster = check;
        monPrefab = _monObj;
        bezierHeight = height;
        fenceCtr = _fenceCtr;
    }

    public void ChatchMonster_Init(Transform _startPos, Transform _endPos, bool check, float height)
    {
        if (!isHaveMonster)
        {
            startPos = _startPos;
            endPos = _endPos;
        }
        isCatchMonster = check;
        bezierHeight = height;
    }

    public GameObject GetMonObj()
    {
        return monObj;
    }



}
