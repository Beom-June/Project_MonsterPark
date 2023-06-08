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
        [SerializeField] private GameObject monBallPrefab;
        [SerializeField] private Transform monBallPos;
        [SerializeField] private Transform backPos;
        [SerializeField] private int level = 1;
        [SerializeField] private int money = 10000;

        [SerializeField] float detectionAngle = 45f;  // ???? ????
        [SerializeField] float detectionRange = 5f;   // ???? ????
        [SerializeField] LayerMask targetLayer;       // ?????? ?????
        //[SerializeField] float alphaColor = 0.3f;
        [SerializeField] Color color;

        private Animator anim;
        private float time;

        private MonsterController monCtr;
        private Collider currentTarget;
        private bool detected = false;
        private bool isMaxItem = false;
        private LineRenderer lr;

        private PlayerController_Inan playerCtr;
        private Transform monsterballTr;

        private float bazierTime = 0.0f;
        private float t = 0.0f;

        [SerializeField] private float bazierSpeed = 2.0f;


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
            if (isMaxItem)
            {
                if (time >= 0)
                {
                    time -= Time.deltaTime;
                }
                anim.SetLayerWeight(1, time);
                Debug.Log("true");
            }

            if (!isMaxItem)
            {
                Collider[] targets = Physics.OverlapSphere(transform.position, detectionRange, targetLayer);
                bool isTargetDetected = targets.Length > 0;

                if (monCtr != null)
                {
                    if (monCtr.GetMonsterIsGrabbed())
                    {
                        MonsterBallMarkerDeactive();
                        monCtr.SetMonsterIsGrabbed(false);
                        monCtr = null;
                        detected = false;

                        GameObject monBallObj = Instantiate(monBallPrefab, monBallPos.position, Quaternion.identity);
                        MonsterBallController monBallCtr = monBallObj.GetComponent<MonsterBallController>();
                        monBallCtr.ChatchMonster_Init(monBallPos, backPos, true, 3.0f);
                        anim.SetLayerWeight(1, time);
                    }
                }

                if (!isTargetDetected)
                {
                    if (time >= 0)
                    {
                        time -= Time.deltaTime * 2.0f;
                    }
                    anim.SetLayerWeight(1, time);

                    if (monCtr != null)
                    {
                        monCtr.CheckLevelLock(level);
                        if (!monCtr.GetLevelLock())
                        {
                            MonsterBallMarkerDeactive();
                            monCtr.SetIsPlayerChase(false);
                        }
                        monCtr.SetLevelLock();
                        monCtr = null;
                        detected = false;
                    }
                }
                else
                {
                    float closestDistance = Mathf.Infinity;
                    foreach (Collider target in targets)
                    {
                        Vector3 directionToTarget = target.transform.position - transform.position;
                        float angle = Vector3.Angle(transform.forward, directionToTarget);

                        if (angle <= detectionAngle * 0.5f)
                        {

                            float distanceToTarget = directionToTarget.magnitude;

                            if (time <= 1)
                            {
                                time += Time.deltaTime * 3f;
                            }
                            anim.SetLayerWeight(1, time);

                            if (distanceToTarget < closestDistance && !detected)
                            {
                                monCtr = target.GetComponent<MonsterController>();
                                currentTarget = target;
                                closestDistance = distanceToTarget;
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
                                    detected = false;

                                    monCtr.SetLevelLock();
                                    monCtr = null;

                                }
                            }

                        }

                    }


                    if (monCtr != null)
                    {
                        monCtr.CheckLevelLock(level);
                        if (!monCtr.GetLevelLock())
                        {
                            monCtr.SetIsPlayerChase(true);
                            MonsterBallMarkerShoot();
                            Debug.Log("추격 시작");
                            detected = true;
                        }

                    }
                }
            }
            
       
           

        }

        void MonsterBallMarkerShoot()
        {
            monsterballTr = playerCtr.GetMonsterBallPos();

            if(monCtr != null)
            {
                lr.positionCount = 2;
                lr.SetPosition(0, monsterballTr.position);

                lr.SetPosition(1, monCtr.transform.position);
            }
            else
            {
                MonsterBallMarkerDeactive();
            }
            
        }


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

        //IEnumerator MoveBallToBack()
        //{
        //    GameObject monBallObj = Instantiate(monBallPrefab, monBallPos.position, Quaternion.identity);
        //    MonsterBallController monBallCtr = monBallObj.GetComponent<MonsterBallController>();
        //    monBallCtr.ChatchMonster_Init(monBallPos, backPos, true);
        //    anim.SetLayerWeight(1, time);

        //    MonsterBallMarkerDeactive();
        //    monCtr = null;
        //    detected = false;
        //    yield return 
        //}

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
                vertices[i + 1] = transform.position + (Vector3.up * 0.01f) + direction * detectionRange; // ????? ?????
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

        public int GetPlayerLevel()
        {   
            return level; 
        }

        public int GetPlayerMoney()
        {   
            return money; 
        }

        public void SetIsMaxItem(bool _isMaxItem)
        {
            isMaxItem = _isMaxItem;
        }
    }

}
