using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster;

namespace Player
{
    public class PlayerSensor : MonoBehaviour
    {
        static public PlayerSensor instance = null;

        [SerializeField] float detectionAngle = 45f;  // 감지 각도
        [SerializeField] float detectionRange = 5f;   // 감지 범위
        [SerializeField] LayerMask targetLayer;       // 감지할 레이어
        //[SerializeField] float alphaColor = 0.3f;
        [SerializeField] Color color;

        private Animator anim;
        private float time;

        private MonsterController monCtr;
        private Collider currentTarget;
        private bool detected = false;

        private LineRenderer lr;

        private PlayerController_Inan playerCtr;
        private Transform monsterballTr;
       

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(this.gameObject);
            }
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            anim = GetComponent<Animator>();
            lr = GetComponent<LineRenderer>();
            playerCtr = GetComponent<PlayerController_Inan>();


            
        }

        private void Start()
        {
            lr.startColor = new Color(1, 0, 0, 1f);
            lr.endColor = new Color(1, 0, 0, 1f);
            lr.startWidth = 0.2f;
            lr.endWidth = 0.1f;
        }

        private void Update()
        {
            // 부채꼴 모양의 감지
            Collider[] targets = Physics.OverlapSphere(transform.position, detectionRange, targetLayer);
            bool isTargetDetected = targets.Length > 0; // 타겟이 감지되었는지 여부 확인

            //잡혔을 때
            if (monCtr != null)
            {
                if (monCtr.GetMonsterIsGrabbed())
                {
                    Debug.Log("몬스터 잡힘");

                    anim.SetLayerWeight(1, time);

                    MonsterBallMarkerDeactive();
                    monCtr = null;
                    detected = false;
                }
            }

   
            //감지 안됐을 때
            if (!isTargetDetected)
            {
                if (time >= 0)
                {
                    time -= Time.deltaTime * 2.0f;
                }
                anim.SetLayerWeight(1, time);

                if (monCtr != null)
                {
                    MonsterBallMarkerDeactive();
                    monCtr.SetIsPlayerChase(false);
                    monCtr = null;
                    detected = false;
                }
            }
            else
            {
                float closestDistance = Mathf.Infinity;
                foreach (Collider target in targets)
                {
                    Vector3 directionToTarget = target.transform.position - transform.position; //target과의 거리
                    float angle = Vector3.Angle(transform.forward, directionToTarget); // 타겟과의 각도

                    if (angle <= detectionAngle * 0.5f)
                    {
        
                        float distanceToTarget = directionToTarget.magnitude;

                        if (time <= 1)
                        {
                            time += Time.deltaTime * 3f;
                        }
                        anim.SetLayerWeight(1, time);

                        // 가장 가까운 타겟의 색상 변경
                        if (distanceToTarget < closestDistance && !detected)
                        {
                            monCtr = target.GetComponent<MonsterController>();
                            currentTarget = target;
                            closestDistance = distanceToTarget;

                            Debug.Log("최단 몬스터 탐색 할당");
                        }
                    }
                    else
                    {
                        if (monCtr != null)
                        {
                            if (currentTarget == target)
                            {
                                if (time >= 0)
                                {
                                    time -= Time.deltaTime;
                                }
                                anim.SetLayerWeight(1, time);

                                MonsterBallMarkerDeactive();
                                monCtr.SetIsPlayerChase(false);
                                monCtr = null;
                                detected = false;
                                Debug.Log("기존 검사하던거 범위에서 벗어남");
                            }
                        }

                    }
       
                }
        

                if (monCtr != null)
                {
                    monCtr.SetIsPlayerChase(true);
                    MonsterBallMarkerShoot();
                    Debug.Log("몬스터 추적");
                    detected = true;
                }
            }

        }

        //몬스터볼 광선 Set
        void MonsterBallMarkerShoot()
        {
            monsterballTr = playerCtr.GetMonsterBallPos();

            lr.positionCount = 2;
            lr.SetPosition(0, monsterballTr.position);

            lr.SetPosition(1, monCtr.transform.position);
        }

        //몬스터볼 광선 초기화
        void MonsterBallMarkerDeactive()
        {
            lr.SetPosition(0, Vector3.zero);
            lr.SetPosition(1, Vector3.zero);

            lr.positionCount = 0;
        }

        private void LateUpdate()
        {   
            if(monCtr != null)
            {
                DrawFanShape();
            }
        }

        private void DrawFanShape()
        {
            int numSegments = 30;
            float angleStep = detectionAngle / numSegments;
            Quaternion startRotation = Quaternion.AngleAxis(-detectionAngle * 0.5f, Vector3.up);

            Vector3[] vertices = new Vector3[numSegments + 2];
            Color[] colors = new Color[numSegments + 2];
            int[] triangles = new int[numSegments * 3];

            vertices[0] = transform.position;
            colors[0] = color;

            for (int i = 0; i <= numSegments; i++)
            {
                Quaternion rotation = startRotation * Quaternion.Euler(0f, i * angleStep, 0f);
                Vector3 direction = rotation * transform.forward;
                vertices[i + 1] = transform.position + (Vector3.up * 0.01f) + direction * detectionRange; // 부채꼴 버텍스
                colors[i + 1] = color;

                if (i < numSegments)
                {
                    int triangleIndex = i * 3;
                    triangles[triangleIndex] = 0;
                    triangles[triangleIndex + 1] = i + 1;
                    triangles[triangleIndex + 2] = i + 2;
                }
            }

            Mesh sensorMesh = new Mesh();
            sensorMesh.vertices = vertices;
            sensorMesh.colors = colors;
            sensorMesh.triangles = triangles;

            Graphics.DrawMesh(sensorMesh, Matrix4x4.identity, new Material(Shader.Find("Sprites/Default")), 0);
        }

        private void OnDrawGizmos()
        {
            // 감지 범위를 시각적으로 표시
            Gizmos.color = color;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-detectionAngle * 0.5f, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(detectionAngle * 0.5f, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;

            Gizmos.DrawRay(transform.position, leftRayDirection * detectionRange);
            Gizmos.DrawRay(transform.position, rightRayDirection * detectionRange);
            Gizmos.DrawRay(transform.position, transform.forward * detectionRange);
            Gizmos.DrawRay(transform.position, -transform.forward * detectionRange);
            Gizmos.DrawRay(transform.position, transform.right * detectionRange);
            Gizmos.DrawRay(transform.position, -transform.right * detectionRange);
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }

}
