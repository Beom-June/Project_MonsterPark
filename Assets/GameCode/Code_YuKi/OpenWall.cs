using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWall : MonoBehaviour
{

    PlayerManager pm;
    [SerializeField] int openLevel;

    // Start is called before the first frame update
    void Start()
    {
        pm = PlayerManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.Level == openLevel)
        {
            gameObject.SetActive(false);
        }
    }
}
