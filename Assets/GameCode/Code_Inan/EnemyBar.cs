using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBar : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.transform.forward);
    }
}
