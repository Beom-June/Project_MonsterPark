using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    PlayerManager pm;
    [SerializeField] private FenceManager[] fences;
    private int fenceIdx = 0;

    public delegate void LevelUp(int level);
    public LevelUp levelUp;

    public static GameManager instance = null;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() 
    {
        pm = PlayerManager.instance;    
    }


    // 레벨 업
    public void PlayerLevelUp()
    {
        int currentLevel = pm.Level;
        int nextLevel = currentLevel + 1;

        // 레벨 업 처리
        pm.Level = nextLevel;
        Debug.Log("Level Up! Current Level: " + nextLevel);
        levelUp.Invoke(nextLevel);
    }

    public void OpenFence()
    {
        Debug.Log("Open Fence " + fenceIdx);
        fenceIdx++;
        fences[fenceIdx].gameObject.SetActive(true);
    }
}
