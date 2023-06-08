using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class CameraController : MonoBehaviour
{

    PlayerControllerMaster playerCtr;
    [SerializeField] float distance;
    [SerializeField] float height;
    [SerializeField] float damping;

    Vector3 velocity;
    void Start()
    {
        playerCtr = PlayerControllerMaster.instance;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = playerCtr.transform.position +
                      (Vector3.back * distance) +
                      (Vector3.up * height);

        transform.position = Vector3.SmoothDamp(transform.position,
            pos,
            ref velocity,
            damping);
    }
}