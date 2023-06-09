using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
 
    PlayerManager pm;
    [SerializeField] private FenceManager[] fences;
    [SerializeField] private FenceController[] FenceCtr = new FenceController[4];
    private int fenceIdx = 0;

    public delegate void LevelUp(int level);
    public LevelUp levelUp;

    public delegate void OpenFenceCamrea(Transform transform);
    public OpenFenceCamrea openFenceCamrea;


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
        levelUp.Invoke(nextLevel);
    }

    public void OpenFence()
    {
        fenceIdx++;
        fences[fenceIdx].gameObject.SetActive(true);
        openFenceCamrea.Invoke(fences[fenceIdx].camTransform);
    }

    

    public FenceController GetFencCtr(int idx)
    {
        return FenceCtr[idx];
    }

    
}
