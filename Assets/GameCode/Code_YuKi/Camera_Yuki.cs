using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class Camera_Yuki : MonoBehaviour
{

    PlayerControllerMaster playerCtr;
    [SerializeField] float distance;
    [SerializeField] float height;
    [SerializeField] float damping;

    private bool isMove = false;
    private Transform target;

    Vector3 velocity;

    GameManager gm;
    void Start()
    {
        playerCtr = PlayerControllerMaster.instance;
        gm = GameManager.instance;

        gm.openFenceCamrea += CameraMoveFlag;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = playerCtr.transform.position +
                      (Vector3.back * distance) +
                      (Vector3.up * height);

        if (!isMove)
        {
            transform.position = Vector3.SmoothDamp(transform.position,
                pos,
                ref velocity,
                damping);
        }
        else
        {
            StartCoroutine(MoveCamera());
        }

    }

    void CameraMoveFlag(Transform transform)
    {
        isMove = true;
        target = transform;
    }

    IEnumerator MoveCamera()
    {   
         transform.position = Vector3.SmoothDamp(transform.position,
                target.position,
                ref velocity,
                damping);

        yield return new WaitForSeconds(1f);
        isMove = false;
    }
    
}